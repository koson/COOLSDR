using CoolComponents;
using CoolSDR.Forms;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Thetis;


namespace CoolSDR.Class
{
    public class Tuning : IVfo
    {
        private readonly FrmMain m_console;
        private readonly FrmMain m_frmMain;
        public IVfo IVFO
        {
            get;
            set;
        }

        public Thetis.RadioBase radio
        {
            get => m_console.radio;
        }
        public Thetis.RadioBase m_radio
        {
            get => m_console.radio;
        }

        public Tuning(FrmMain console)
        {
            m_console = console;
            m_frmMain = console;
            InitPreamps();
            InitOffsets();
        }

        public void FrequencyChanged(double newFreqMHz)
        {
            ChangeFrequency(newFreqMHz);
        }

        double tx_dds_freq_mhz = 14;
        public void UpdateTXDDSFreq(double freq = -1)
        {
            if (freq > 0)
                tx_dds_freq_mhz = freq; // update freq here

            if (m_console.mox)
            {
                // fixme
                // SetAlexHPF(fwc_dds_freq);
                // SetAlexLPF(tx_dds_freq_mhz);
            }
            NetworkIO.VFOfreq(0, tx_dds_freq_mhz, 1);
        }

        public void SetAlexHPF(double freq)
        {
            if (m_console == null) return;
            NetworkIO.SetAlexHPFBits(0x20); // Bypass HPF

            // Debug.Assert(false);
            /*/
            if (CurrentHPSDRHardware == HPSDRHW.OrionMKII)
                SetBPF1(freq);
            else
            {
                if (alexpresent && !initializing)
                {
                    if (mox && disable_hpf_on_tx)
                    {
                        NetworkIO.SetAlexHPFBits(0x20);
                        SetupForm.radDHPFTXled.Checked = true;
                        return;
                    }

                    if (alex_hpf_bypass)
                    {
                        NetworkIO.SetAlexHPFBits(0x20); // Bypass HPF
                        SetupForm.radBPHPFled.Checked = true;
                        return;
                    }

                    if ((decimal)freq >= SetupForm.udAlex1_5HPFStart.Value
                        && // 1.5 MHz HPF
                        (decimal)freq <= SetupForm.udAlex1_5HPFEnd.Value)
                    {
                        if (alex1_5bphpf_bypass)
                        {
                            NetworkIO.SetAlexHPFBits(0x20); // Bypass HPF
                            SetupForm.radBPHPFled.Checked = true;
                        }
                        else
                        {
                            NetworkIO.SetAlexHPFBits(0x10);
                            SetupForm.rad1_5HPFled.Checked = true;
                        }
                    }

                    else if ((decimal)freq >= SetupForm.udAlex6_5HPFStart.Value
                        && // 6.5 MHz HPF
                        (decimal)freq <= SetupForm.udAlex6_5HPFEnd.Value)
                    {
                        if (alex6_5bphpf_bypass)
                        {
                            NetworkIO.SetAlexHPFBits(0x20); // Bypass HPF
                            SetupForm.radBPHPFled.Checked = true;
                        }
                        else
                        {
                            NetworkIO.SetAlexHPFBits(0x08);
                            SetupForm.rad6_5HPFled.Checked = true;
                        }
                    }

                    else if ((decimal)freq >= SetupForm.udAlex9_5HPFStart.Value
                        && // 9.5 MHz HPF
                        (decimal)freq <= SetupForm.udAlex9_5HPFEnd.Value)
                    {
                        if (alex9_5bphpf_bypass)
                        {
                            NetworkIO.SetAlexHPFBits(0x20); // Bypass HPF
                            SetupForm.radBPHPFled.Checked = true;
                        }
                        else
                        {
                            NetworkIO.SetAlexHPFBits(0x04);
                            SetupForm.rad9_5HPFled.Checked = true;
                        }
                    }

                    else if ((decimal)freq >= SetupForm.udAlex13HPFStart.Value
                        && // 13 MHz HPF
                        (decimal)freq <= SetupForm.udAlex13HPFEnd.Value)
                    {
                        if (alex13bphpf_bypass)
                        {
                            NetworkIO.SetAlexHPFBits(0x20); // Bypass HPF
                            SetupForm.radBPHPFled.Checked = true;
                        }
                        else
                        {
                            NetworkIO.SetAlexHPFBits(0x01);
                            SetupForm.rad13HPFled.Checked = true;
                        }
                    }

                    else if ((decimal)freq >= SetupForm.udAlex20HPFStart.Value
                        && // 20 MHz HPF
                        (decimal)freq <= SetupForm.udAlex20HPFEnd.Value)
                    {
                        if (alex20bphpf_bypass)
                        {
                            NetworkIO.SetAlexHPFBits(0x20); // Bypass HPF
                            SetupForm.radBPHPFled.Checked = true;
                        }
                        else
                        {
                            NetworkIO.SetAlexHPFBits(0x02);
                            SetupForm.rad20HPFled.Checked = true;
                        }
                    }

                    else if ((decimal)freq >= SetupForm.udAlex6BPFStart.Value
                        && // 6m BPF/LNA
                        (decimal)freq <= SetupForm.udAlex6BPFEnd.Value)
                    {
                        if (alex6bphpf_bypass || disable_6m_lna_on_rx
                            || (mox && disable_6m_lna_on_tx))
                        {
                            NetworkIO.SetAlexHPFBits(0x20); // Bypass HPF
                            SetupForm.radBPHPFled.Checked = true;
                        }
                        else
                        {
                            NetworkIO.SetAlexHPFBits(0x40);
                            SetupForm.rad6BPFled.Checked = true;
                        }
                    }
                    else
                    {
                        NetworkIO.SetAlexHPFBits(0x20); // Bypass HPF
                        SetupForm.radBPHPFled.Checked = true;
                    }
                }
            }
            /*/
        }

        private void UpdateAlexTXFilter()
        {
            //Debug.Assert(false);
            // fixme
        }

        private void UpdateAlexRXFilter()
        {
            // Debug.Assert(false);
            // fixme
        }

        public float[] rx1_preamp_offset; // offset values for each preamp mode in
                                          // dB
        public float[] rx2_preamp_offset; // offset values for each preamp mode in
                                          // dB
        public float rx1_meter_cal_offset; // multimeter calibration offset per
                                           // volume setting in dB
        public float rx2_meter_cal_offset; // multimeter calibration offset per
                                           // volume setting in dB
        public float[] rx_meter_cal_offset_by_radio;
        public float[] rx_display_cal_offset_by_radio;


        internal Band rx1_band = new Band();


        private double vfo_offset = 0.0;
        public double VFOOffset
        {
            get { return vfo_offset; }
            set { vfo_offset = value; }
        }

        private double rx2_vfo_offset = 0.0;
        public double RX2VFOOffset
        {
            get { return rx2_vfo_offset; }
            set { rx2_vfo_offset = value; }
        }

        internal float rx2_dds_freq;

        public double FWCDDSFreq
        {
            get { return fwc_dds_freq; }
            set
            {
                if (m_frmMain == null) return;
                fwc_dds_freq = value;

                double f = fwc_dds_freq + vfo_offset;
                rx1_dds_freq_mhz = f;
                UpdateRX1DDSFreq();
            }
        }

        internal bool rx2_enabled = false;
        internal bool alexpresent = true;
        internal double rx1_dds_freq_mhz;



        internal double fwc_dds_freq = 14;

        void InitPreamps()
        {
            rx1_preamp_offset = new float[(int)PreampMode.LAST];

            rx1_preamp_offset[(int)PreampMode.HPSDR_OFF] = 20.0f; // atten inline
            rx1_preamp_offset[(int)PreampMode.HPSDR_ON] = 0.0f; // no atten
            rx1_preamp_offset[(int)PreampMode.HPSDR_MINUS10] = 10.0f;
            rx1_preamp_offset[(int)PreampMode.HPSDR_MINUS20] = 20.0f;
            rx1_preamp_offset[(int)PreampMode.HPSDR_MINUS30] = 30.0f;
            rx1_preamp_offset[(int)PreampMode.HPSDR_MINUS40] = 40.0f;
            rx1_preamp_offset[(int)PreampMode.HPSDR_MINUS50] = 50.0f;
            rx1_preamp_offset[(int)PreampMode.SA_MINUS10] = 10f;
            rx1_preamp_offset[(int)PreampMode.SA_MINUS30] = 30f;

            rx2_preamp_offset = new float[8];
            rx2_preamp_offset[(int)PreampMode.HPSDR_OFF] = 20.0f;
            rx2_preamp_offset[(int)PreampMode.HPSDR_ON] = 0.0f;
            rx2_preamp_offset[(int)PreampMode.HPSDR_MINUS10] = 10.0f;
            rx2_preamp_offset[(int)PreampMode.HPSDR_MINUS30] = 30.0f;
        }

        void InitOffsets()
        {

            rx_meter_cal_offset_by_radio = new float[(int)HPSDRModel.LAST];
            rx_display_cal_offset_by_radio = new float[(int)HPSDRModel.LAST];
            for (int i = 0; i < (int)HPSDRModel.LAST; i++)
            {
                switch ((HPSDRModel)i)
                {
                    case HPSDRModel.ANAN7000D:
                    case HPSDRModel.ANAN8000D:
                    case HPSDRModel.ORIONMKII:
                        rx_meter_cal_offset_by_radio[i] = 4.841644f;
                        rx_display_cal_offset_by_radio[i] = 5.259f;
                        break;
                    default:
                        rx_meter_cal_offset_by_radio[i] = 0.98f;
                        rx_display_cal_offset_by_radio[i] = -2.1f;
                        break;
                }
            }
        }

        double rx2_dds_freq_mhz;
        private void UpdateRX2DDSFreq()
        {

            UpdateAlexTXFilter();
            UpdateAlexRXFilter();

            if (current_hpsdr_model == HPSDRModel.ORIONMKII
                || current_hpsdr_model == HPSDRModel.ANAN7000D
                || current_hpsdr_model == HPSDRModel.ANAN8000D)
            {
                SetAlex2HPF(rx2_dds_freq_mhz);
            }

            switch (CurrentHPSDRModel)
            {
                case HPSDRModel.HERMES:
                case HPSDRModel.ANAN10:
                case HPSDRModel.ANAN10E:
                case HPSDRModel.ANAN100:
                case HPSDRModel.ANAN100B:
                    NetworkIO.VFOfreq(1, rx2_dds_freq_mhz, 0);
                    break;
                default: NetworkIO.VFOfreq(3, rx2_dds_freq_mhz, 0); break;
            }
        }


        private void UpdateTXDDSFreq()
        {
            if (mox)
            {
                SetAlexHPF(fwc_dds_freq);
                SetAlexLPF(tx_dds_freq_mhz);
            }
            NetworkIO.VFOfreq(0, tx_dds_freq_mhz, 1);
        }



        private readonly String[] on_off_preamp_settings = { "0dB", "-20dB" };
        private readonly String[] anan100d_preamp_settings
            = { "0dB", "-10dB", "-20dB", "-30dB" };
        // private String[] alex_preamp_settings = { "0db", "-20db",
        // "-10dB",
        // "-20dB", "-30dB", "-40dB",
        // "-50dB" };
        private readonly String[] alex_preamp_settings
            = { "-10db", "-20db", "-30db", "-40db", "-50db" };
        public void UpdateRX1DDSFreq()
        {

            SetAlexHPF(fwc_dds_freq);
            UpdateAlexTXFilter();
            UpdateAlexRXFilter();

            switch (CurrentHPSDRModel)
            {
                case HPSDRModel.HERMES:
                case HPSDRModel.ANAN10:
                case HPSDRModel.ANAN10E:
                case HPSDRModel.ANAN100:
                case HPSDRModel.ANAN100B:
                    NetworkIO.VFOfreq(0, rx1_dds_freq_mhz, 0);
                    break;
                default:
                    NetworkIO.VFOfreq(0, rx1_dds_freq_mhz, 0);
                    NetworkIO.VFOfreq(1, rx1_dds_freq_mhz, 0);
                    NetworkIO.VFOfreq(2, rx1_dds_freq_mhz, 0);
                    break;
            }
        }

        private bool shown_warning = false;
        public void SetAlexLPF(double freq)
        {
            // Fixme
            if (!shown_warning)
            {
                Debug.Print("**** FIXME ****** SetAlexLPF is hardcoded for 160m");
                shown_warning = true;
            }
            NetworkIO.SetAlexLPFBits(0x08);

        }

        HPSDRModel current_hpsdr_model
        {
            get => radio.CurrentRadio;
        }

        public bool mox
        {
            get
            {
                return m_console.mox;
            }
        }

        System.Windows.Forms.ComboBox comboMeterTXMode
        {
            get => m_console.comboMeterTXMode;
        }

        System.Windows.Forms.ComboBox comboPreamp
        {
            get { return m_console.comboPreamp; }
        }
        public void SetComboPreampForHPSDR()
        {

            comboPreamp.Items.Clear();

            switch (m_radio.CurrentRadio)
            {
                case HPSDRModel.HPSDR:
                    comboPreamp.Items.AddRange(on_off_preamp_settings);
                    if (alexpresent)
                    {
                        comboPreamp.Items.AddRange(alex_preamp_settings);
                    }

                    break;
                case HPSDRModel.HERMES:
                    if (alexpresent)
                    {
                        comboPreamp.Items.AddRange(on_off_preamp_settings);
                        comboPreamp.Items.AddRange(alex_preamp_settings);
                    }
                    else
                        comboPreamp.Items.AddRange(anan100d_preamp_settings);

                    break;
                case HPSDRModel.ANAN10:
                case HPSDRModel.ANAN10E:
                    comboPreamp.Items.AddRange(anan100d_preamp_settings);
                    break;
                case HPSDRModel.ANAN100:
                case HPSDRModel.ANAN100B:
                    comboPreamp.Items.AddRange(on_off_preamp_settings);
                    comboPreamp.Items.AddRange(alex_preamp_settings);
                    break;
                case HPSDRModel.ANAN100D:
                case HPSDRModel.ANAN200D:
                    if (alexpresent)
                    {
                        comboPreamp.Items.AddRange(on_off_preamp_settings);
                        comboPreamp.Items.AddRange(alex_preamp_settings);
                    }
                    else
                        comboPreamp.Items.AddRange(anan100d_preamp_settings);
                    break;
                case HPSDRModel.ANAN7000D:
                case HPSDRModel.ANAN8000D:
                case HPSDRModel.ORIONMKII:
                    comboPreamp.Items.AddRange(anan100d_preamp_settings);
                    break;
            }

            /*/
            comboRX2Preamp.Items.Clear();
            if (current_hpsdr_model == HPSDRModel.ANAN100D
                || current_hpsdr_model == HPSDRModel.ANAN200D
                || current_hpsdr_model == HPSDRModel.ANAN7000D
                || current_hpsdr_model == HPSDRModel.ANAN8000D
                || current_hpsdr_model == HPSDRModel.ORIONMKII)
                comboRX2Preamp.Items.AddRange(anan100d_preamp_settings);
            else
                comboRX2Preamp.Items.AddRange(on_off_preamp_settings);

            /*/

            RX1PreampMode = m_radio.rx1_preamp_by_band[(int)rx1_band];
            // this.comboPreamp.SelectedIndexChanged(this, EventArgs.Empty)
            RX1AttenuatorData = m_radio.rx1_step_attenuator_by_band[(int)rx1_band];
        }
        public void UpdatePreamps()
        {
            if (current_hpsdr_model == HPSDRModel.HPSDR)
            {
                update_preamp = false;
                update_preamp_mode = false;
                return;
            }

            if (!mox && m_frmMain.ATTOnTX)
            {
                if (update_preamp_mode && !update_preamp_mutex)
                {
                    update_preamp_mutex = true;
                    RX1PreampMode = preamp;
                    Debug.Assert(false);
                    //SetupForm.HermesEnableAttenuator = old_satt;
                    //SetupForm.HermesAttenuatorData = old_satt_data;

                    update_preamp_mode = false;
                    update_preamp_mutex = false;
                }

                if (update_preamp && !update_preamp_mutex)
                {

                    preamp = RX1PreampMode; // save current preamp mode
                                            // Debug.Assert(false);
                    old_satt_data = HermesAttenuatorData;
                    old_satt = rx1_step_att_present;
                    update_preamp = false;
                }
            }
        }

        private bool update_preamp_mutex = false;
        private PreampMode preamp;
        // private PreampMode rx2_preamp;
        private bool old_satt = false;
        private int old_satt_data = 0;
        // private bool old_rx2_satt = false;
        //  private int old_rx2_satt_data = 0;
        private bool update_preamp = true;
        private bool update_preamp_mode = false;

        internal void UpdateDDCs(bool rX2Enabled)
        {
            return; // diversity stuff. // fixme
        }

        internal System.Windows.Forms.NumericUpDown udRX1StepAttData
        {
            get
            {
                return m_console.udRX1StepAttData;

            }
        }
        public int HermesAttenuatorData
        {
            get
            {

                if (this.udRX1StepAttData != null)
                    return (int)udRX1StepAttData.Value;
                else
                    return -1;
            }
            set
            {
                if (udRX1StepAttData != null)
                    udRX1StepAttData.Value = value;
            }
        }

        public void SetAlex2HPF(double freq)
        {
            /*/
            if (alexpresent)
            {
                // JanusAudio.SetAlexManEnable(0x01);

                if (alex2_hpf_bypass)
                {
                    NetworkIO.SetAlex2HPFBits(0x20); // Bypass HPF
                    SetupForm.radAlex2BPHPFled.Checked = true;
                    return;
                }

                if ((decimal)freq >= SetupForm.udAlex21_5HPFStart.Value
                    && // 1.5 MHz HPF
                    (decimal)freq <= SetupForm.udAlex21_5HPFEnd.Value)
                {
                    if (alex21_5bphpf_bypass)
                    {
                        NetworkIO.SetAlex2HPFBits(0x20); // Bypass HPF
                        SetupForm.radAlex2BPHPFled.Checked = true;
                    }
                    else
                    {
                        NetworkIO.SetAlex2HPFBits(0x10);
                        SetupForm.radAlex21_5HPFled.Checked = true;
                    }
                }

                else if ((decimal)freq >= SetupForm.udAlex26_5HPFStart.Value
                    && // 6.5 MHz HPF
                    (decimal)freq <= SetupForm.udAlex26_5HPFEnd.Value)
                {
                    if (alex26_5bphpf_bypass)
                    {
                        NetworkIO.SetAlex2HPFBits(0x20); // Bypass HPF
                        SetupForm.radAlex2BPHPFled.Checked = true;
                    }
                    else
                    {
                        NetworkIO.SetAlex2HPFBits(0x08);
                        SetupForm.radAlex26_5HPFled.Checked = true;
                    }
                }

                else if ((decimal)freq >= SetupForm.udAlex29_5HPFStart.Value
                    && // 9.5 MHz HPF
                    (decimal)freq <= SetupForm.udAlex29_5HPFEnd.Value)
                {
                    if (alex29_5bphpf_bypass)
                    {
                        NetworkIO.SetAlex2HPFBits(0x20); // Bypass HPF
                        SetupForm.radAlex2BPHPFled.Checked = true;
                    }
                    else
                    {
                        NetworkIO.SetAlex2HPFBits(0x04);
                        SetupForm.radAlex29_5HPFled.Checked = true;
                    }
                }

                else if ((decimal)freq >= SetupForm.udAlex213HPFStart.Value
                    && // 13 MHz HPF
                    (decimal)freq <= SetupForm.udAlex213HPFEnd.Value)
                {
                    if (alex213bphpf_bypass)
                    {
                        NetworkIO.SetAlex2HPFBits(0x20); // Bypass HPF
                        SetupForm.radAlex2BPHPFled.Checked = true;
                    }
                    else
                    {
                        NetworkIO.SetAlex2HPFBits(0x01);
                        SetupForm.radAlex213HPFled.Checked = true;
                    }
                }

                else if ((decimal)freq >= SetupForm.udAlex220HPFStart.Value
                    && // 20 MHz HPF
                    (decimal)freq <= SetupForm.udAlex220HPFEnd.Value)
                {
                    if (alex220bphpf_bypass)
                    {
                        NetworkIO.SetAlex2HPFBits(0x20); // Bypass HPF
                        SetupForm.radAlex2BPHPFled.Checked = true;
                    }
                    else
                    {
                        NetworkIO.SetAlex2HPFBits(0x02);
                        SetupForm.radAlex220HPFled.Checked = true;
                    }
                }

                else if ((decimal)freq >= SetupForm.udAlex26BPFStart.Value
                    && // 6m BPF/LNA
                    (decimal)freq <= SetupForm.udAlex26BPFEnd.Value)
                {
                    if (alex26bphpf_bypass)
                    {
                        NetworkIO.SetAlex2HPFBits(0x20); // Bypass HPF
                        SetupForm.radAlex2BPHPFled.Checked = true;
                    }
                    else
                    {
                        NetworkIO.SetAlex2HPFBits(0x40);
                        SetupForm.radAlex26BPFled.Checked = true;
                    }
                }
                else
                {
                    NetworkIO.SetAlex2HPFBits(0x20); // Bypass HPF
                    SetupForm.radAlex2BPHPFled.Checked = true;
                }
            }
            /*/ // fixme
        }





        private float rx1_display_cal_offset; // display calibration offset
                                              // per volume setting in dB
        public float RX1DisplayCalOffset
        {
            get { return rx1_display_cal_offset; }
            set
            {
                rx1_display_cal_offset = value;
                RX2DisplayCalOffset = value;
                //  UpdateDisplayOffsets();
            }
        }

        private float rx2_display_cal_offset; // display calibration offset
                                              // per volume setting in dB
        public float RX2DisplayCalOffset
        {
            get { return rx2_display_cal_offset; }
            set
            {
                rx2_display_cal_offset = value;
                // Display.RX2DisplayCalOffset = value;
                //  UpdateDisplayOffsets();
            }
        }

        public bool rx2_preamp_present { get; private set; }


        public HPSDRModel CurrentHPSDRModel
        {
            get
            {
                if (m_radio == null)
                {
                    return HPSDRModel.HERMES;
                }

                return m_radio.CurrentRadio;
            }
            set
            {
                Debug.Assert(m_frmMain != null);
                if (m_frmMain == null) return;
                m_radio.CurrentRadio = value;
                HPSDRModel current_hpsdr_model = value;

                Display.CurrentHPSDRModel = value;
                NetworkIO.fwVersionsChecked = false;

                switch (current_hpsdr_model)
                {
                    case HPSDRModel.HERMES:

                        rx2_preamp_present = false;
                        NetworkIO.SetRxADC(1);
                        NetworkIO.SetMKIIBPF(0);
                        cmaster.SetADCSupply(0, 33);
                        NetworkIO.LRAudioSwap(1);
                        m_radio.CurrentHPSDRHardware = HPSDRHW.Hermes;
                        break;
                    case HPSDRModel.ANAN10:

                        rx2_preamp_present = false;
                        NetworkIO.SetRxADC(1);
                        NetworkIO.SetMKIIBPF(0);
                        cmaster.SetADCSupply(0, 33);
                        NetworkIO.LRAudioSwap(1);
                        m_radio.CurrentHPSDRHardware = HPSDRHW.Hermes;
                        break;
                    case HPSDRModel.ANAN10E:
                        rx2_preamp_present = false;
                        NetworkIO.SetRxADC(1);
                        NetworkIO.SetMKIIBPF(0);
                        cmaster.SetADCSupply(0, 33);
                        NetworkIO.LRAudioSwap(1);
                        m_radio.CurrentHPSDRHardware = HPSDRHW.HermesII;
                        break;
                    case HPSDRModel.ANAN100:
                        rx2_preamp_present = false;
                        NetworkIO.SetRxADC(1);
                        NetworkIO.SetMKIIBPF(0);
                        cmaster.SetADCSupply(0, 33);
                        NetworkIO.LRAudioSwap(1);
                        m_radio.CurrentHPSDRHardware = HPSDRHW.Hermes;
                        break;
                    case HPSDRModel.ANAN100B:
                        rx2_preamp_present = false;
                        NetworkIO.SetRxADC(1);
                        NetworkIO.SetMKIIBPF(0);
                        cmaster.SetADCSupply(0, 33);
                        NetworkIO.LRAudioSwap(1);
                        m_radio.CurrentHPSDRHardware = HPSDRHW.HermesII;
                        break;
                    case HPSDRModel.ANAN100D:
                        rx2_preamp_present = true;
                        NetworkIO.SetRxADC(2);
                        NetworkIO.SetMKIIBPF(0);
                        cmaster.SetADCSupply(0, 33);
                        NetworkIO.LRAudioSwap(0);
                        m_radio.CurrentHPSDRHardware = HPSDRHW.Angelia;
                        break;
                    case HPSDRModel.ANAN200D:
                        rx2_preamp_present = true;
                        NetworkIO.SetRxADC(2);
                        NetworkIO.SetMKIIBPF(0);
                        cmaster.SetADCSupply(0, 50);
                        NetworkIO.LRAudioSwap(0);
                        m_radio.CurrentHPSDRHardware = HPSDRHW.Orion;
                        break;
                    case HPSDRModel.ORIONMKII:
                        rx2_preamp_present = true;
                        NetworkIO.SetRxADC(2);
                        NetworkIO.SetMKIIBPF(1);
                        cmaster.SetADCSupply(0, 50);
                        NetworkIO.LRAudioSwap(0);
                        m_radio.CurrentHPSDRHardware = HPSDRHW.OrionMKII;
                        break;
                    case HPSDRModel.ANAN7000D:

                        rx2_preamp_present = true;
                        NetworkIO.SetRxADC(2);
                        NetworkIO.SetMKIIBPF(1);
                        cmaster.SetADCSupply(0, 50);
                        NetworkIO.LRAudioSwap(0);
                        m_radio.CurrentHPSDRHardware = HPSDRHW.OrionMKII;
                        break;
                    case HPSDRModel.ANAN8000D:
                        rx2_preamp_present = true;
                        NetworkIO.SetRxADC(2);
                        NetworkIO.SetMKIIBPF(1);
                        cmaster.SetADCSupply(0, 50);
                        NetworkIO.LRAudioSwap(0);
                        m_radio.CurrentHPSDRHardware = HPSDRHW.OrionMKII;
                        break;
                }

                switch (current_hpsdr_model)
                {
                    case HPSDRModel.HPSDR: break;
                    case HPSDRModel.HERMES:
                    case HPSDRModel.ANAN10:
                    case HPSDRModel.ANAN10E:
                    case HPSDRModel.ANAN100:
                    case HPSDRModel.ANAN100B:
                    case HPSDRModel.ANAN100D:
                    case HPSDRModel.ANAN200D:
                    case HPSDRModel.ORIONMKII:
                    case HPSDRModel.ANAN7000D:
                    case HPSDRModel.ANAN8000D:
                        /*/ fixme: metering
                        if (!comboMeterTXMode.Items.Contains("Ref Pwr"))
                            comboMeterTXMode.Items.Insert(1, "Ref Pwr");
                        if (!comboMeterTXMode.Items.Contains("SWR"))
                            comboMeterTXMode.Items.Insert(2, "SWR");
                        if (!comboMeterTXMode.Items.Contains("Fwd SWR"))
                            comboMeterTXMode.Items.Insert(3, "Fwd SWR");
                        /*/
                        break;
                }

                SetComboPreampForHPSDR();
                UpdateDDCs(rx2_enabled);


                {
                    rx1_meter_cal_offset
                        = rx_meter_cal_offset_by_radio[(int)current_hpsdr_model];
                    RX1DisplayCalOffset
                        = rx_display_cal_offset_by_radio[(int)current_hpsdr_model];
                    rx2_meter_cal_offset
                        = rx_meter_cal_offset_by_radio[(int)current_hpsdr_model];
                    RX2DisplayCalOffset
                        = rx_display_cal_offset_by_radio[(int)current_hpsdr_model];
                }

                //if (!IsSetupFormNull && saved_hpsdr_model != current_hpsdr_model)
                //  txtVFOAFreq_LostFocus(this, EventArgs.Empty);

                cmaster.CMSetTXOutputLevelRun();
            }
        }

        private int rx1_attenuator_data = 0;

        public bool AlexPresent
        {
            get { return alexpresent; }
            set
            {
                alexpresent = value;

                if (alexpresent)
                {
                    if (!comboMeterTXMode.Items.Contains("Ref Pwr"))
                        comboMeterTXMode.Items.Insert(1, "Ref Pwr");

                    if (!comboMeterTXMode.Items.Contains("SWR"))
                        comboMeterTXMode.Items.Insert(2, "SWR");

                    if (comboMeterTXMode.SelectedIndex < 0)
                        comboMeterTXMode.SelectedIndex = 0;

                    SetAlexHPF(fwc_dds_freq);
                    SetAlexLPF(tx_dds_freq_mhz);
                    //SetAlex2HPF(rx2_dds_freq_mhz);
                }
                else
                {
                    string cur_txt = comboMeterTXMode.Text;


                    {
                        if (comboMeterTXMode.Items.Contains("Ref Pwr"))
                            comboMeterTXMode.Items.Remove("Ref Pwr");
                        if (comboMeterTXMode.Items.Contains("SWR"))
                            comboMeterTXMode.Items.Remove("SWR");
                    }

                    comboMeterTXMode.Text = cur_txt;
                    if (comboMeterTXMode.SelectedIndex < 0
                        && comboMeterTXMode.Items.Count > 0)
                        comboMeterTXMode.SelectedIndex = 0;
                }
            }
        }
        public int RX1AttenuatorData
        {
            get { return rx1_attenuator_data; }
            set
            {
                if (m_frmMain == null) return;
                rx1_attenuator_data = value;


                var current_hpsdr_model = m_radio.CurrentRadio;

                if (AlexPresent && current_hpsdr_model != HPSDRModel.ANAN10
                    && current_hpsdr_model != HPSDRModel.ANAN10E
                    && current_hpsdr_model != HPSDRModel.ANAN7000D
                    && current_hpsdr_model != HPSDRModel.ANAN8000D
                    && current_hpsdr_model != HPSDRModel.ORIONMKII)
                    udRX1StepAttData.Maximum = (decimal)61;
                else
                    udRX1StepAttData.Maximum = (decimal)31;

                if (rx1_step_att_present)
                {
                    if (AlexPresent && current_hpsdr_model != HPSDRModel.ANAN10
                        && current_hpsdr_model != HPSDRModel.ANAN10E
                        && current_hpsdr_model != HPSDRModel.ANAN7000D
                        && current_hpsdr_model != HPSDRModel.ANAN8000D
                        && current_hpsdr_model != HPSDRModel.ORIONMKII)
                    {
                        if (rx1_attenuator_data <= 31)
                        {
                            NetworkIO.SetAlexAtten(0); // 0dB Alex Attenuator
                            NetworkIO.SetADC1StepAttenData(rx1_attenuator_data);
                        }
                        else
                        {
                            NetworkIO.SetAlexAtten(3); // -30dB Alex Attenuator
                            NetworkIO.SetADC1StepAttenData(rx1_attenuator_data + 2);
                        }
                    }
                    else
                    {
                        NetworkIO.SetAlexAtten(0);
                        NetworkIO.SetADC1StepAttenData(rx1_attenuator_data);
                    }
                }

                if (!m_frmMain.mox)
                    m_radio.rx1_step_attenuator_by_band[(int)rx1_band] = rx1_attenuator_data;

                if (rx1_attenuator_data >= udRX1StepAttData.Minimum)
                {
                    udRX1StepAttData.Value = rx1_attenuator_data;
                }
                //lblAttenLabel.Text = rx1_attenuator_data.ToString() + " dB";
                if (!m_frmMain.mox)
                {
                    update_preamp = true;
                    UpdatePreamps();
                }
                UpdateRX1DisplayOffsets();

            }
        }

        private float rx1_xvtr_gain_offset = 0;
        private float rx1_6m_gain_offset = 0;
        private float rx1_path_offset = 0;
        private void UpdateRX1DisplayOffsets()
        {


            if (rx1_step_att_present)
            {
                Display.RX1PreampOffset = rx1_attenuator_data;
                if (current_hpsdr_model != HPSDRModel.ANAN100D
                        && current_hpsdr_model != HPSDRModel.ANAN200D
                        && current_hpsdr_model != HPSDRModel.ORIONMKII
                        && current_hpsdr_model != HPSDRModel.ANAN7000D
                        && current_hpsdr_model != HPSDRModel.ANAN8000D
                        && !rx2_preamp_present
                    || m_console.mox)
                    Display.RX2PreampOffset = rx1_attenuator_data;
            }
            else
            {
                Display.RX1PreampOffset = rx1_preamp_offset[(int)rx1_preamp_mode];

                if (current_hpsdr_model != HPSDRModel.ANAN100D
                    && current_hpsdr_model != HPSDRModel.ANAN200D
                    && current_hpsdr_model != HPSDRModel.ORIONMKII
                    && current_hpsdr_model != HPSDRModel.ANAN7000D
                    && current_hpsdr_model != HPSDRModel.ANAN8000D
                    && !rx2_preamp_present)
                    Display.RX2PreampOffset
                        = rx1_preamp_offset[(int)rx1_preamp_mode];
            }

            switch (Display.CurrentDisplayMode)
            {
                case DisplayMode.WATERFALL:
                case DisplayMode.PANADAPTER:
                case DisplayMode.PANAFALL:
                case DisplayMode.PANASCOPE:
                case DisplayMode.SPECTRUM:
                case DisplayMode.HISTOGRAM:
                case DisplayMode.SPECTRASCOPE:
                    Display.RX1DisplayCalOffset = rx1_display_cal_offset
                        + rx1_xvtr_gain_offset + rx1_6m_gain_offset;
                    break;
                default:
                    Display.RX1DisplayCalOffset = rx1_display_cal_offset
                        + rx1_path_offset + rx1_xvtr_gain_offset;
                    break;
            }
        }

        private bool rx1_step_att_present = false;
        public bool RX1StepAttPresent
        {
            get { return rx1_step_att_present; }
            set
            {
                if (m_frmMain == null) return;
                rx1_step_att_present = value;
                Debug.Assert(false);
                /*/
                if (rx1_step_att_present)
                {
                    lblPreamp.Text = "S-ATT";
                    udRX1StepAttData.BringToFront();
                    udRX1StepAttData_ValueChanged(this, EventArgs.Empty);
                    // NetworkIO.EnableADC1StepAtten(1);
                }
                else
                {
                    lblPreamp.Text = "ATT";
                    comboPreamp.BringToFront();
                    comboPreamp_SelectedIndexChanged(this, EventArgs.Empty);
                    //  NetworkIO.EnableADC1StepAtten(1);

                    if (AlexPresent)
                        NetworkIO.SetAlexAtten(
                            alex_atten); // normal up alex attenuator setting
                }

                ////MW0LGE why ??  if (CollapsedDisplay)
                //    CollapseDisplay();

                if (!mox)
                {
                    // update_preamp_mode = false;
                    update_preamp = true;
                    UpdatePreamps();
                }
                UpdateRX1DisplayOffsets();
                /*/
            }
        }

        private int alex_atten;
        private PreampMode rx1_preamp_mode = PreampMode.HPSDR_OFF;

        public PreampMode RX1PreampMode
        {
            get { return rx1_preamp_mode; }
            set
            {
                if (m_frmMain == null) return;
                rx1_preamp_mode = value;


                if (!alexpresent
                    && ((rx1_preamp_mode == PreampMode.HPSDR_MINUS10)
                        || (rx1_preamp_mode == PreampMode.HPSDR_MINUS20)
                        || (rx1_preamp_mode == PreampMode.HPSDR_MINUS30)
                        || (rx1_preamp_mode == PreampMode.HPSDR_MINUS40)
                        || (rx1_preamp_mode == PreampMode.HPSDR_MINUS50)))
                {
                    rx1_preamp_mode = PreampMode.HPSDR_OFF;
                }

                alex_atten = 0;
                int merc_preamp = 0;
                int rx1_att_value = 0;
                switch (rx1_preamp_mode)
                {
                    case PreampMode.HPSDR_ON: // 0dB
                        rx1_att_value = 0;
                        merc_preamp = 1; // no attn
                        alex_atten = 0;
                        break;
                    case PreampMode.HPSDR_OFF: //-20dB
                        rx1_att_value = 20;
                        merc_preamp = 0; // attn inline
                        alex_atten = 0;
                        break;
                    case PreampMode.HPSDR_MINUS10:
                        rx1_att_value = 0;
                        merc_preamp = 1;
                        alex_atten = 1;
                        break;
                    case PreampMode.HPSDR_MINUS20:
                        rx1_att_value = 0;
                        merc_preamp = 1;
                        alex_atten = 2;
                        break;
                    case PreampMode.HPSDR_MINUS30:
                        rx1_att_value = 0;
                        merc_preamp = 1;
                        alex_atten = 3;
                        break;
                    case PreampMode.HPSDR_MINUS40:
                        rx1_att_value = 20;
                        merc_preamp = 0;
                        alex_atten = 2;
                        break;
                    case PreampMode.HPSDR_MINUS50:
                        rx1_att_value = 20;
                        merc_preamp = 0;
                        alex_atten = 3;
                        break;
                    case PreampMode.SA_MINUS10:
                        rx1_att_value = 10;
                        merc_preamp = 0;
                        alex_atten = 0;
                        break;
                    case PreampMode.SA_MINUS30:
                        rx1_att_value = 30;
                        merc_preamp = 0;
                        alex_atten = 0;
                        break;
                }
                if (m_console.radio.CurrentRadio != HPSDRModel.HPSDR)
                {
                    if (!rx1_step_att_present)
                    {
                        NetworkIO.SetADC1StepAttenData(rx1_att_value);
                    }
                }
                else
                {
                    NetworkIO.SetRX1Preamp(merc_preamp);
                }

                NetworkIO.SetAlexAtten(alex_atten);
                m_radio.rx1_preamp_by_band[(int)rx1_band] = rx1_preamp_mode;

                switch (rx1_preamp_mode)
                {
                    case PreampMode.HPSDR_ON: comboPreamp.Text = "0dB"; break;

                    case PreampMode.HPSDR_OFF: comboPreamp.Text = "-20dB"; break;

                    case PreampMode.HPSDR_MINUS10: comboPreamp.Text = "-10db"; break;

                    case PreampMode.HPSDR_MINUS20: comboPreamp.Text = "-20db"; break;

                    case PreampMode.HPSDR_MINUS30: comboPreamp.Text = "-30db"; break;

                    case PreampMode.HPSDR_MINUS40: comboPreamp.Text = "-40dB"; break;

                    case PreampMode.HPSDR_MINUS50: comboPreamp.Text = "-50dB"; break;

                    case PreampMode.SA_MINUS10: comboPreamp.Text = "-10dB"; break;

                    case PreampMode.SA_MINUS30: comboPreamp.Text = "-30dB"; break;
                }

                UpdateRX1DisplayOffsets();

                //if (chkSquelch.Checked) ptbSquelch_Scroll(this, EventArgs.Empty);

                if (!mox)
                {
                    update_preamp = true;
                    UpdatePreamps();
                }
            }
        }



        public void ChangeFrequency(double newFreqMHz)
        {
            UpdateTXDDSFreq(newFreqMHz);
            rx1_dds_freq_mhz = newFreqMHz;
            UpdateRX1DDSFreq();
            rx2_dds_freq_mhz = newFreqMHz;
            UpdateRX2DDSFreq();
            UpdateTXDDSFreq();
            SetAlexHPF(newFreqMHz);
            SetAlexLPF(newFreqMHz);
        }


        private bool alex_hpf_bypass = false;
        public bool AlexHPFBypass
        {
            get { return alex_hpf_bypass; }
            set
            {
                alex_hpf_bypass = value;
                double freq = rx1_dds_freq_mhz;
                SetAlexHPF(freq);
                // if (!initializing) txtVFOAFreq_LostFocus(this, EventArgs.Empty);
                // BPF1ToolStripMenuItem.Checked = value;
            }
        }
    }
}
