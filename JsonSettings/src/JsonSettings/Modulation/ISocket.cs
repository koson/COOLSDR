﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Nucs.JsonSettings.Modulation {
    /// <summary>
    ///     A class that can be attached to and deattached from with <see cref="Module"/>s.
    /// </summary>
    public interface ISocket {
        /// <summary>
        ///     Attach a module to current socket.
        /// </summary>
        void Attach(Module t);
        /// <summary>
        ///     Deattach a module from any socket it was attached to.<br></br>This is merely a shortcut to <see cref="Module.Deattach"/>.
        /// </summary>
        void Deattach(Module t);
        IReadOnlyList<Module> Modules { get; }
        bool IsAttached(Func<Module, bool> checker);
        bool IsAttachedOfType<T>() where T : Module;
        bool IsAttachedOfType(Type t);
        
    }
}