﻿// Copyright (c) 2010-2013 Anders Gustafsson, Cureos AB.
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.

using System;
using System.Linq;
using Windows.Networking;
using Windows.Networking.Connectivity;

// ReSharper disable CheckNamespace
namespace Override.System
// ReSharper restore CheckNamespace
{
    public static class Environment
    {
        ///<summary>
        /// Want to store the hostname to send for push notifications to make
        /// the management UI better. Take the substring up to the first period
        /// of the first DomainName entry.
        /// 
        /// Thanks to Jeff Wilcox and Matthijs Hoekstra
        /// Adapted from Q42.WinRT library at https://github.com/Q42/Q42.WinRT
        ///</summary>
        public static string MachineName
        {
            get
            {
                var list = NetworkInformation.GetHostNames().ToArray();
                string name = null;
                if (list.Length > 0)
                {
                    foreach (var entry in list)
                    {
                        if (entry.Type == HostNameType.DomainName)
                        {
                            var s = entry.CanonicalName;
                            if (!String.IsNullOrEmpty(s))
                            {
                                // Domain-joined. Requires at least a one-character name.
                                var j = s.IndexOf('.');

                                if (j > 0)
                                {
                                    name = s.Substring(0, j);
                                    break;
                                }

                                // Typical home machine.
                                name = s;
                            }
                        }
                    }
                }

                if (String.IsNullOrEmpty(name))
                {
                    // TODO: Localize?
                    name = "Unknown Windows 8";
                }

                return name;
            }
        }
    }
}