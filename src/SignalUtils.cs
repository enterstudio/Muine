/*
 * Copyright (C) 2004 Tamara Roberson <tamara.roberson@gmail.com>
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as
 * published by the Free Software Foundation; either version 2 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public
 * License along with this program; if not, write to the
 * Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
 * Boston, MA 02110-1301, USA.
 */

using System;
using System.Runtime.InteropServices;

namespace Muine
{
	public static class SignalUtils 
	{
		// Delegates
		public delegate void SignalDelegate    (IntPtr obj);
		public delegate void SignalDelegatePtr (IntPtr obj, IntPtr arg);
		public delegate void SignalDelegateInt (IntPtr obj, int    arg);
		public delegate void SignalDelegateStr (IntPtr obj, string arg);

		// Methods
		// Methods :: Public
		// Methods :: Public :: SignalConnect
		// Methods :: Public :: SignalConnect :: Plain
		[DllImport ("libgobject-2.0-0.dll")]
		private static extern uint g_signal_connect_data
		  (IntPtr obj, string name, SignalDelegate cb, IntPtr data, IntPtr p,
		   int flags);

		public static uint SignalConnect
		  (IntPtr obj, string name, SignalDelegate cb)
		{
			return SignalConnect (obj, name, cb, IntPtr.Zero, IntPtr.Zero, 0);
		}

		public static uint SignalConnect
		  (IntPtr obj, string name, SignalDelegate cb, IntPtr data, IntPtr p,
		   int flags)
		{
			return g_signal_connect_data (obj, name, cb, data, p, flags);
		}

		// Methods :: Public :: SignalConnect :: Ptr
		[DllImport ("libgobject-2.0-0.dll")]
		private static extern uint g_signal_connect_data
		  (IntPtr obj, string name, SignalDelegatePtr cb, IntPtr data, 
		   IntPtr p, int flags);

		public static uint SignalConnect
		  (IntPtr obj, string name, SignalDelegatePtr cb)
		{
			return SignalConnect (obj, name, cb, IntPtr.Zero, IntPtr.Zero, 0);
		}
	        
		public static uint SignalConnect
		  (IntPtr obj, string name, SignalDelegatePtr cb, IntPtr data,
		   IntPtr p, int flags)
		{
			return g_signal_connect_data (obj, name, cb, data, p, flags);
		}

		// Methods :: Public :: SignalConnect :: Int
		[DllImport ("libgobject-2.0-0.dll")]
		private static extern uint g_signal_connect_data
		  (IntPtr obj, string name, SignalDelegateInt cb, IntPtr data,
		   IntPtr p, int flags);

		public static uint SignalConnect
		  (IntPtr obj, string name, SignalDelegateInt cb)
		{
			return SignalConnect (obj, name, cb, IntPtr.Zero, IntPtr.Zero, 0);
		}

		public static uint SignalConnect
		  (IntPtr obj, string name, SignalDelegateInt cb, IntPtr data,
		   IntPtr p, int flags)
		{
			return g_signal_connect_data (obj, name, cb, data, p, flags);
		}

		// Methods :: Public :: SignalConnect :: Str
		[DllImport ("libgobject-2.0-0.dll")]
		private static extern uint g_signal_connect_data
		  (IntPtr obj, string name, SignalDelegateStr cb, IntPtr data,
		   IntPtr p, int flags);

		public static uint SignalConnect
		  (IntPtr obj, string name, SignalDelegateStr cb)
		{
			return SignalConnect (obj, name, cb, IntPtr.Zero, IntPtr.Zero, 0);
		}

		public static uint SignalConnect
		  (IntPtr obj, string name, SignalDelegateStr cb, IntPtr data,
		   IntPtr p, int flags)
		{
			return g_signal_connect_data (obj, name, cb, data, p, flags);
		}
	}
}
