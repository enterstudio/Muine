Name: muine
Version: 0.8.11
Release: 1

Group: Applications/Multimedia
License: GPL
URL: http://people.nl.linux.org/~jorn/Muine/

Source: %{name}-%{version}.tar.gz
BuildRoot: %{_tmppath}/%{name}-root
Requires: /sbin/ldconfig, mono-core, gtksharp2 libogg libvorbis flac
Requires: gstreamer
BuildRequires: libogg-devel libvorbis-devel, mono-devel, gtksharp2-gapi, libid3tag-devel, flac-devel, mono-web
BuildRequires: gstreamer-devel
BuildRequires: gstreamer-plugins-devel

Summary: GNOME music player
%description
Muine is a new music player using some new UI ideas. The idea is that it will be much easier and comfortable to use than the iTunes model, which is used by both Rhythmbox and Jamboree. It is written in C and C#, using GStreamer for music playback.

%prep
%setup -q

%build
%configure --disable-gac-install \
	   --enable-gstreamer=yes

make %{_smp_mflags}

%install
rm -rf %{buildroot}
export GCONF_DISABLE_MAKEFILE_SCHEMA_INSTALL=1
%makeinstall

%clean
rm -rf %{buildroot}

%post
/sbin/ldconfig
export GCONF_CONFIG_SOURCE=`gconftool-2 --get-default-source`
gconftool-2 --makefile-install-rule \
    %{_sysconfdir}/gconf/schemas/muine.schemas > /dev/null;

%postun -p /sbin/ldconfig

%preun
export GCONF_CONFIG_SOURCE=`gconftool-2 --get-default-source`
gconftool-2 --makefile-uninstall-rule \
    %{_sysconfdir}/gconf/schemas/muine.schemas >/dev/null;

%files
%defattr(-, root, root)
%doc AUTHORS COPYING ChangeLog NEWS README TODO
%{_bindir}/*
%{_sysconfdir}/*
%{_libdir}/%{name}/*
%{_datadir}/locale/*
%{_datadir}/pixmaps/%{name}.png
%{_datadir}/applications/%{name}.desktop
%{_libdir}/dbus-1.0
%{_libdir}/mono/gac
%{_libdir}/pkgconfig/*.pc

%changelog
* Thu Jan 13 2005 Christian Schaller <Uraeus@gnome.org
- Make build work with latest library versions and names
- Remove chooseable backend stuff as it do not work for me, someone
  who knows how to make it work can put it back in

* Wed Jun 30 2004 Alastair Porter <alastair@linuxexperience.com>
- Fix so only xine or gstreamer is required.

* Mon Jun 21 2004 Alastair Porter <alastair@linuxexperience.com>
- Change mono dependencies to the Ximan/Novel ones
- Make GConf installation work.

* Sat Jan 31 2004 Jorn Baayen <jorn@nl.linux.org>
- Add a build dependency on flac-devel

* Thu Jan 22 2004 Link Dupont <link@subpop.net> 0.2-2.spc
- Add explicit dependency on Mono.

* Thu Jan 22 2004 Link Dupont <link@subpop.net> 0.2-1.spc
- Update to 0.2.

* Tue Jan 20 2004 Link Dupont <link@subpop.net> 0.1.1-1.spc
- Initial RPM release.
- Rereleased for spc RPMs.
