Muine
=====

Muine is an innovative music player, featuring a simple, intuitive interface.
It is designed to allow users to easily construct a playlist from albums and/or
single songs. Its goal is to be simply a music player, not to become a robust
music management application. This doesn't mean Muine has no features! Some
feature highlights:

 * Ogg/Vorbis, FLAC, AAC and MP3 music playback support
 * Automatic album cover fetching via MusicBrainz and Amazon
 * Support for embedded album images in ID3v2 tags
 * ReplayGain support
 * Support for multiple artist and performer tags per song
 * Plug-in support
 * Translations into many languages

Muine is targeted at the GNOME desktop and uses GTK+ for the interface. Most of
the code is written in C#, with some additions/bindings/glue in plain C. Muine
was originally written by Jorn Baayen, but now maintained mostly by others.


Build and Installation Requirements
===================================

Basic requirements:
  Mono >= 1.1
  Gtk# >= 2.6
  Gtk+ >= 2.6
  Taglib-Sharp >= 2.0.3
  gdbm

Playback support (one of the options below):
  GStreamer 0.10           (used by default)
  xine-lib >= 1.0.0rc3b
