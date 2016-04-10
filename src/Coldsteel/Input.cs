﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Coldsteel
{
    public class Input
    {
        private Dictionary<string, Control> _controls = new Dictionary<string, Control>();

        /// <summary>
        /// Gets all Controls.
        /// </summary>
        public IEnumerable<Control> Controls { get { return _controls.Values; } }

        /// <summary>
        /// Adds a control to the input.
        /// </summary>
        /// <param name="control"></param>
        public void AddControl(Control control)
        {
            if (_controls.ContainsKey(control.Key))
                throw new ArgumentException(String.Format("duplicate control key {0}", control.Key));

            _controls.Add(control.Key, control);
        }

        /// <summary>
        /// Gets a control by its key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Control GetControl(string key)
        {
            if (!_controls.ContainsKey(key))
                throw new ArgumentException(String.Format("there is no registered control with Key {0}", key));

            return _controls[key];
        }
    }
}