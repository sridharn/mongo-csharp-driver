﻿/* Copyright 2010-2012 10gen Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Driver
{
    /// <summary>
    /// Represents a setting that may or may not have been set.
    /// </summary>
    public struct Setting<T>
    {
        // private fields
        private T _value;
        private bool _hasBeenSet;

        // public properties
        /// <summary>
        /// Gets the value of the settings.
        /// </summary>
        public T Value
        {
            get { return _value; }
            set { 
                _value = value;
                _hasBeenSet = true;
            }
        }

        /// <summary>
        /// Gets whether the setting has been set.
        /// </summary>
        public bool HasBeenSet
        {
            get { return _hasBeenSet; }
        }

        // public methods
        /// <summary>
        /// Resets the setting to the unset state.
        /// </summary>
        public void Reset()
        {
            _value = default(T);
            _hasBeenSet = false;
        }

        /// <summary>
        /// Gets a canonical string representation for this setting.
        /// </summary>
        /// <returns>A canonical string representation for this setting.</returns>
        public override string ToString()
        {
            return _hasBeenSet ? _value.ToString() : "default";
        }

        // internal methods
        internal Setting<T> Clone()
        {
            var clone = new Setting<T>();
            clone._value = _value;
            clone._hasBeenSet = _hasBeenSet;
            return clone;
        }
    }
}