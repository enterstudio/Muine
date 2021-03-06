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
	public class Database
	{
		// Static
		// Static :: Methods
		// Static :: Methods :: Pack
		// Static :: Methods :: Pack :: PackStart
		[DllImport ("libmuine")]
		private static extern IntPtr db_pack_start ();

		/// <summary>
		///	Get a location in memory where we can pack values.
		/// </summary>
		/// <returns>
		///	An <see cref="IntPtr" / to where to start packing values.
		/// </returns>
		public static IntPtr PackStart ()
		{
			return db_pack_start ();
		}
		
		// Static :: Methods :: Pack :: PackEnd
		[DllImport ("libmuine")]
		private static extern IntPtr db_pack_end (IntPtr p, out int length);

		/// <summary>
		///	Finish packing.
		/// </summary>
		/// <param name="p">
		///	An <see cref="IntPtr" /> to where the last value ends.
		/// </param>
		/// <param name="length">
		///	Location to store the length of the data that has been
		///	packed.
		/// </param>
		/// <returns>
		///	An <see cref="IntPtr" /> to the end of the packed values.
		/// </returns>
		public static IntPtr PackEnd (IntPtr p, out int length)
		{
			return db_pack_end (p, out length);
		}
		
		// Static :: Methods :: Pack :: PackPixbuf
		//	TODO: Take a Gdk.Pixbuf as an argument, rather than
		//	its handle.		
		[DllImport ("libmuine")]
		private static extern void db_pack_pixbuf (IntPtr p, IntPtr pixbuf);

		/// <summary>
		///	Pack a <see cref="Gdk.Pixbuf" /> so it can be stored in
		/// 	the database.
		/// </summary>
		/// <param name="p">
		///	An <see cref="IntPtr" /> to where the value should be stored.
		/// </param>
		/// <param name="pixbuf">
		///	An <see cref="IntPtr" /> to the <see cref="Gdk.Pixbuf" />.
		/// </param>
		public static void PackPixbuf (IntPtr p, IntPtr pixbuf)
		{
			db_pack_pixbuf (p, pixbuf);	
		}
		
		// Static :: Methods :: Pack :: PackString
		[DllImport ("libmuine")]
		private static extern void db_pack_string (IntPtr p, string str);
		
		/// <summary>
		///	Pack a <see cref="String" /> so it can be stored in
		/// 	the database.
		/// </summary>
		/// <remarks>
		///	The string is packed as:
		///	length + string + null.
		/// </remarks>
		/// <param name="p">
		///	An <see cref="IntPtr" /> to where the value should be stored.
		/// </param>
		/// <param name="str">
		///	A <see cref="String" />.
		/// </param>
		public static void PackString (IntPtr p, string str)
		{
			db_pack_string (p, str);
		}

		// Static :: Methods :: Pack :: PackStringArray
		/// <summary>
		///	Pack an array of <see cref="String">strings</see> so they
		///	can be stored in the database.
		/// </summary>
		/// <remarks>
		///	The array is packed as:
		///	length + strings.
		///	Note that each string ends with a null character.
		/// </remarks>
		/// <param name="p">
		///	An <see cref="IntPtr" /> to where the value should be stored.
		/// </param>
		/// <param name="str">
		///	An array of <see cref="String">strings</see>.
		/// </param>
		public static void PackStringArray (IntPtr p, string [] array)
		{
			Database.PackInt (p, array.Length);
			foreach (string s in array)
				Database.PackString (p, s);
		}
		
		// Static :: Methods :: Pack :: PackInt
		[DllImport ("libmuine")]
		private static extern void db_pack_int (IntPtr p, int i);

		/// <summary>
		///	Pack an <see cref="Int32">integer</see> so it can be
		///	stored in the database.
		/// </summary>
		/// <param name="p">
		///	An <see cref="IntPtr" /> to where the value should be stored.
		/// </param>
		/// <param name="i">
		///	An <see cref="Int32">integer</see>.
		/// </param>
		public static void PackInt (IntPtr p, int i)
		{
			db_pack_int (p, i);
		}
		
		// Static :: Methods :: Pack :: PackBool
		[DllImport ("libmuine")]
		private static extern void db_pack_bool (IntPtr p, bool b);

		/// <summary>
		///	Pack a <see cref="Boolean" /> so it can be stored in
		/// 	the database.
		/// </summary>
		/// <param name="p">
		///	An <see cref="IntPtr" /> to where the value should be stored.
		/// </param>
		/// <param name="b">
		///	A <see cref="Boolean" />.
		/// </param>
		public static void PackBool (IntPtr p, bool b)
		{
			db_pack_bool (p, b);
		}
				
		// Static :: Methods :: Pack :: PackDouble
		[DllImport ("libmuine")]
		private static extern void db_pack_double (IntPtr p, double d);

		/// <summary>
		///	Pack a <see cref="Double" /> so it can be stored in
		/// 	the database.
		/// </summary>
		/// <param name="p">
		///	An <see cref="IntPtr" /> to where the value should be stored.
		/// </param>
		/// <param name="d">
		///	A <see cref="Double" />.
		/// </param>
		public static void PackDouble (IntPtr p, double d)
		{
			db_pack_double (p, d);
		}		

		// Static :: Methods :: Unpack
		// Static :: Methods :: Unpack :: UnpackBool
		[DllImport ("libmuine")]
		private static extern IntPtr db_unpack_bool (IntPtr p, out bool b);

		/// <summary>
		///	Unpack a <see cref="Boolean" /> from the database.
		/// </summary>
		/// <param name="p">
		///	An <see cref="IntPtr" /> to where the value is stored.
		/// </param>
		/// <param name="b">
		///	Location to store the unpacked value.
		/// </param>
		/// <returns>
		///	An <see cref="IntPtr" /> to where the end of the value
		/// 	is stored.
		/// </returns>
		public static IntPtr UnpackBool (IntPtr p, out bool b)
		{
			return db_unpack_bool (p, out b);        
		}

		// Static :: Methods :: Unpack :: UnpackDouble
		[DllImport ("libmuine")]
		private static extern IntPtr db_unpack_double (IntPtr p, out double d);

		/// <summary>
		///	Unpack a <see cref="Double" /> from the database.
		/// </summary>
		/// <param name="p">
		///	An <see cref="IntPtr" /> to where the value is stored.
		/// </param>
		/// <param name="d">
		///	Location to store the unpacked value.
		/// </param>
		/// <returns>
		///	An <see cref="IntPtr" /> to where the end of the value
		/// 	is stored.
		/// </returns>
		public static IntPtr UnpackDouble (IntPtr p, out double d)
		{
			return db_unpack_double (p, out d);
		}

		// Static :: Methods :: Unpack :: UnpackInt
		[DllImport ("libmuine")]
		private static extern IntPtr db_unpack_int (IntPtr p, out int i);

		/// <summary>
		///	Unpack an <see cref="Int32">integer</see> from the database.
		/// </summary>
		/// <param name="p">
		///	An <see cref="IntPtr" /> to where the value is stored.
		/// </param>
		/// <param name="i">
		///	Location to store the unpacked value.
		/// </param>
		/// <returns>
		/// An <see cref="IntPtr" /> to where the end of the value is stored.
		/// </returns>
		public static IntPtr UnpackInt (IntPtr p, out int i)
		{
			return db_unpack_int (p, out i);
		}

		// Static :: Methods :: Unpack :: UnpackPixbuf
		//	TODO: Return a Gdk.Pixbuf in the out parameter.
		[DllImport ("libmuine")]
		private static extern IntPtr db_unpack_pixbuf
		  (IntPtr p, out IntPtr pixbuf);

		/// <summary>
		///	Unpack a <see cref="Gdk.Pixuf" /> from the database.
		/// </summary>
		/// <param name="p">
		///	A <see cref="IntPtr" /> to where the value is stored.
		/// </param>
		/// <param name="pixbuf">
		///	Location to store the <see cref="IntPtr" /> to the
		///	unpacked <see cref="Gdk.Pixbuf" />.
		/// </param>
		/// <returns>
		///	An <see cref="IntPtr" /> to where the end of the value
		/// 	is stored.
		/// </returns>
		public static IntPtr UnpackPixbuf (IntPtr p, out IntPtr pixbuf)
		{
			return db_unpack_pixbuf (p, out pixbuf);
		}

		// Static :: Methods :: Unpack :: UnpackString
		//	TODO: Merge the second overload into the first one since
		//	that is the only place that uses it.
		[DllImport ("libmuine")]
		private static extern IntPtr db_unpack_string
		  (IntPtr p, out IntPtr str_ptr);

		/// <summary>
		///	Unpack a <see cref="String" /> from the database.
		/// </summary>
		/// <param name="p">
		///	An <see cref="IntPtr" /> to where the value is stored.
		/// </param>
		/// <param name="str">
		///	Location to store the unpacked value.
		/// </param>
		/// <returns>
		///	An <see cref="IntPtr" /> to where the end of the value
		/// 	is stored.
		/// </returns>
		public static IntPtr UnpackString (IntPtr p, out string str)
		{
			IntPtr str_ptr;
			IntPtr ret = UnpackString (p, out str_ptr); 
			str = GLib.Marshaller.PtrToStringGFree (str_ptr);
			return ret;
		}

		/// <summary>
		///	Unpack a <see cref="String" /> from the database.
		/// </summary>
		/// <param name="p">
		///	An <see cref="IntPtr" /> to where the value is stored.
		/// </param>
		/// <param name="str_ptr">
		///	Location to store the <see cref="IntPtr" /> to the 
		///	unpacked value.
		/// </param>
		/// <returns>
		///	An <see cref="IntPtr" /> to where the end of the value
		///	is stored.
		/// </returns>
		public static IntPtr UnpackString (IntPtr p, out IntPtr str_ptr)
		{
			return db_unpack_string (p, out str_ptr);
		}

		// Static :: Methods :: Unpack :: UnpackStringArray
		/// <summary>
		///	Unpack an array of <see cref="String">strings</see> from
		///	the database.
		/// </summary>
		/// <param name="p">
		///	An <see cref="IntPtr" /> to where the value is stored.
		/// </param>
		/// <param name="array">
		///	Location to store the unpacked value.
		/// </param>
		/// <returns>
		///	An <see cref="IntPtr" /> to where the end of the value
		/// 	is stored.
		/// </returns>
		public static IntPtr UnpackStringArray (IntPtr p, out string [] array)
		{
			IntPtr ret = p;

			int len;
			ret = Database.UnpackInt (ret, out len);

			array = new string [len];

			for (int i = 0; i < len; i++)
				ret = Database.UnpackString (ret, out array [i]);

			return ret;
		}

		// Delegates
		public delegate void DecodeFunctionDelegate (string key, IntPtr data);

		// Variables
		private IntPtr db_ptr;

		// Constructor
		[DllImport ("libmuine")]
		private static extern IntPtr db_open
		  (string filename, int version, out IntPtr error);

		/// <summary>
		///	Creates a new <see cref="Database" /> object.
		/// </summary>
		/// <param name="filename">
		///	The location of the database.
		/// </param>
		/// <param name="version">
		///	The version of the database which we support.
		/// </param>
		/// <exception cref="Exception">
		///	Thrown if the database cannot be opened.
		/// </exception>
		public Database (string filename, int version)
		{
			IntPtr error_ptr;

			db_ptr = db_open (filename, version, out error_ptr); 

			if (db_ptr == IntPtr.Zero) {
				string msg = GLib.Marshaller.PtrToStringGFree (error_ptr);
				throw new Exception (msg);
			}
		}

		// Properties
		// Properties :: Handle (get;)
		/// <summary>
		///	Contains the <see cref="IntPtr">handle</see> of the
		///	database object.
		/// </summary>
		/// <remarks>
		///	This is used to communicate with libmuine.
		/// </remarks>
		public IntPtr Handle {
			get { return db_ptr; }
		}

		// Methods
		// Methods :: Public
		// Methods :: Public :: Load
		[DllImport ("libmuine")]
		private static extern void db_foreach (IntPtr db_ptr, 
						       DecodeFunctionDelegate decode_function, 
						       IntPtr data);

		/// <summary>
		///	Load the database.
		/// </summary>
		/// <param name="decode_function">
		///	The delegate used to decode the database.
		/// </param>
		public void Load (DecodeFunctionDelegate decode_function)
		{
			db_foreach (db_ptr, decode_function, IntPtr.Zero);
		}
			
		// Methods :: Public :: Store
		[DllImport ("libmuine")]
		private static extern void db_store
		  (IntPtr db_ptr, string key, bool overwrite, IntPtr data,
		   int data_size);

		/// <summary>
		///	Store a key-value pair in the database. If the key
		///	already exists, do not overwrite.
		/// </summary>
		/// <param name="key">
		///	The key.
		/// </param>
		/// <param name="data">
		///	An <see cref="IntPtr" /> to where the data is stored.
		/// </param>
		/// <param name="data_size">
		///	The size of the data to store.
		/// </param>
		public void Store (string key, IntPtr data, int data_size)
		{
			Store (key, data, data_size, false);
		}
		
		/// <summary>
		///	Store a key-value pair in the database.
		/// </summary>
		/// <param name="key">
		///	The key.
		/// </param>
		/// <param name="data">
		///	An <see cref="IntPtr" /> to where the data is stored.
		/// </param>
		/// <param name="data_size">
		///	The size of the data to store.
		/// </param>
		/// <param name="overwrite">
		///	Whether to overwrite if the key already exists.
		/// </param>
		public void Store
		  (string key, IntPtr data, int data_size, bool overwrite)
		{
			db_store (db_ptr, key, overwrite, data, data_size);
		} 

		// Methods :: Public :: Delete
		[DllImport ("libmuine")]
		private static extern void db_delete (IntPtr db_ptr, string key);

		/// <summary>
		///	Remove a key-value pair from the database.
		/// </summary>
		/// <param name="key">
		///	The key to remove.
		/// </param>
		public void Delete (string key)
		{
			db_delete (db_ptr, key);
		}
	} 
}
