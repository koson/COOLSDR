// This is an independent project of an individual developer. Dear PVS-Studio,
// please check it.

// PVS-Studio Static Code Analyzer for C, C++, C#, and Java:
// http://www.viva64.com
/*  cmbuffs.c

This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2014 Warren Pratt, NR0V

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

The author can be reached by email at

warren@wpratt.com

*/

#include "cmcomm.h"

void start_cmthread(int id) {
    CMB a = pcm->pcbuff[id];
    a->hCMThread = (HANDLE)_beginthread(cm_main, 0, (void*)id);
}

void create_cmbuffs(
    int id, int accept, int max_insize, int max_outsize, int outsize) {

    CMB existing = pcm->pcbuff[id];
    assert(!existing); // no need to create if it already exists: KLJ
    CMB a = (CMB)malloc0(sizeof(cmb));

    pcm->pcbuff[id] = pcm->pdbuff[id] = pcm->pebuff[id] = pcm->pfbuff[id] = a;
    a->id = id;
    a->accept = accept;
    a->run = 1;
    a->max_in_size = max_insize;
    a->max_outsize = max_outsize;
    a->r1_outsize = outsize;
    if (a->max_outsize > a->max_in_size)
        a->r1_size = a->max_outsize;
    else
        a->r1_size = a->max_in_size;
    a->r1_active_buffsize = CMB_MULT * a->r1_size;
    a->r1_baseptr
        = (double*)malloc0(a->r1_active_buffsize * sizeof(WDSP_COMPLEX));
    a->r1_inidx = 0;
    a->r1_outidx = 0;
    a->r1_unqueuedsamps = 0;
    a->Sem_BuffReady = CreateSemaphore(0, 0, 1000, 0);
    InitializeCriticalSectionAndSpinCount(&a->csIN, 2500);
    InitializeCriticalSectionAndSpinCount(&a->csOUT, 2500);
    start_cmthread(id);
}

void destroy_cmbuffs(int id) {

    CMB a = pcm->pcbuff[id];
    InterlockedBitTestAndReset(
        &a->accept, 0); // shut the Inbound() gate to prevent new infusions
    EnterCriticalSection(
        &a->csIN); // wait until the current Inbound() infusion is finished
    EnterCriticalSection(&a->csOUT); // block the CM thread before cmdata()
    Sleep(25); // wait for the thread to arrive at the top of the cm_main() loop
    InterlockedBitTestAndReset(&a->run, 0); // set a trap for the CM thread
    ReleaseSemaphore(a->Sem_BuffReady, 1,
        0); // be sure the CM thread can pass WaitForSingleObject in cm_main()
            // //
    LeaveCriticalSection(
        &a->csOUT); // let the thread pass to the trap in cmdata()
    Sleep(2); // wait for the CM thread to die
    DeleteCriticalSection(&a->csOUT);
    DeleteCriticalSection(&a->csIN);
    CloseHandle(a->Sem_BuffReady);
    _aligned_free(a->r1_baseptr);
    _aligned_free(a);
}

void flush_cmbuffs(int id) {
    CMB a = pcm->pfbuff[id];
    memset(a->r1_baseptr, 0, a->r1_active_buffsize * sizeof(WDSP_COMPLEX));
    a->r1_inidx = 0;
    a->r1_outidx = 0;
    a->r1_unqueuedsamps = 0;
    while (!WaitForSingleObject(a->Sem_BuffReady, 1))
        ;
}

PORT void Inbound(int id, int nsamples, double* in) {
    int n;
    int first, second;
    CMB a = pcm->pebuff[id];

    if (_InterlockedAnd(&a->accept, 1)) {
        EnterCriticalSection(&a->csIN);
        if (nsamples > (a->r1_active_buffsize - a->r1_inidx)) {
            first = a->r1_active_buffsize - a->r1_inidx;
            second = nsamples - first;
        } else {
            first = nsamples;
            second = 0;
        }
        memcpy(
            a->r1_baseptr + 2 * a->r1_inidx, in, first * sizeof(WDSP_COMPLEX));
        memcpy(a->r1_baseptr, in + 2 * first, second * sizeof(WDSP_COMPLEX));

        if ((a->r1_unqueuedsamps += nsamples) >= a->r1_outsize) {
            n = a->r1_unqueuedsamps / a->r1_outsize;
            a->when_sembuffready = timeGetTime();
            ReleaseSemaphore(a->Sem_BuffReady, n, 0);
            a->r1_unqueuedsamps -= n * a->r1_outsize;
        }
        if ((a->r1_inidx += nsamples) >= a->r1_active_buffsize)
            a->r1_inidx -= a->r1_active_buffsize;
        LeaveCriticalSection(&a->csIN);
    }
}

// KLJ: returns < 1 if the thread is to end
int cmdata(int id, double* out) {
    int first, second;
    CMB a = pcm->pdbuff[id];
    EnterCriticalSection(&a->csOUT);
    if (!_InterlockedAnd(&a->run, 1)) {
        LeaveCriticalSection(&a->csOUT);
        return -1;
    }
    if (a->r1_outsize > (a->r1_active_buffsize - a->r1_outidx)) {
        first = a->r1_active_buffsize - a->r1_outidx;
        second = a->r1_outsize - first;
    } else {
        first = a->r1_outsize;
        second = 0;
    }
    memcpy(out, a->r1_baseptr + 2 * a->r1_outidx, first * sizeof(WDSP_COMPLEX));
    memcpy(out + 2 * first, a->r1_baseptr, second * sizeof(WDSP_COMPLEX));
    if ((a->r1_outidx += a->r1_outsize) >= a->r1_active_buffsize)
        a->r1_outidx -= a->r1_active_buffsize;
    LeaveCriticalSection(&a->csOUT);
    return 0;
}
#ifdef DEBUG_TIMINGS
static unsigned int times_ctr = 0;
#endif

void cm_main(void* pargs) {

    HANDLE hpri = prioritise_thread_max();
    int id = (int)(ptrdiff_t)pargs;

    CMB a = pcm->pdbuff[id];
    while (_InterlockedAnd(&a->run, 1)) {

        DWORD dwWait = WaitForSingleObject(a->Sem_BuffReady, 500);
        if (dwWait == WAIT_TIMEOUT) {
            continue;
        }
        if (cmdata(id, pcm->in[id]) >= 0) {
            xcmaster(id);
        } else {
            if (hpri) {
                prioritise_thread_cleanup(hpri);
            }
            printf("cm_main, with id: %ld and hCMThread: %p exited.\n", a->id,
                a->hCMThread);
            fflush(stdout);
            a->hCMThread = 0;
        }
    }
}

void SetCMRingOutsize(int id, int size) {
    CMB a = pcm->pcbuff[id];
    InterlockedBitTestAndReset(
        &a->accept, 0); // shut the Inbound() gate to prevent new infusions
    EnterCriticalSection(
        &a->csIN); // wait until the current Inbound() infusion is finished
    EnterCriticalSection(&a->csOUT); // block the CM thread before cmdata()
    Sleep(25); // wait for the thread to arrive at the top of the cm_main() loop
    InterlockedBitTestAndReset(&a->run, 0); // set a trap for the CM thread
    ReleaseSemaphore(a->Sem_BuffReady, 1,
        0); // be sure the CM thread can pass WaitForSingleObject in cm_main()
            // //
    LeaveCriticalSection(
        &a->csOUT); // let the thread pass to the trap in cmdata()

    // make sure the thread has finished, KLJ
    int slept = 0;
    volatile int tid = GetThreadId(a->hCMThread);
    volatile int tid_local = tid;
    while (a->hCMThread != 0) {
        Sleep(1);
        slept++;
        tid_local = GetThreadId(a->hCMThread);
        if (tid_local == 0 || (LONG_PTR)tid_local == INVALID_HANDLE_VALUE) {
            if (a->hCMThread) {
                assert(0);
                // break;
            }
        }
        if (slept > 1000) break;
    }
    //////////////////////////////////////////////////////////////////////////
    // KLJ this was just Sleep(2)m not actually waiting for the thread to exit
    // Sleep(2); // wait for the CM thread to die/////////////////////////////
    flush_cmbuffs(id); // restore ring to pristine condition
    a->r1_outsize = size; // set its new outsize
    InterlockedBitTestAndSet(&a->run, 0); // remove the CM thread trap
    start_cmthread(id); // start the CM thread
    LeaveCriticalSection(&a->csIN); // enable Inbound() processing
    InterlockedBitTestAndSet(&a->accept, 0); // open the Inbound() gate
}
