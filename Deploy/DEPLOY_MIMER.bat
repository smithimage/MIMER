7z a ..\MIMER_Source%1.zip ..\*.* -x!*.svn -x!*.suo -x!MIMER\bin -x!MIMER\obj -x!MIMER\*.*scc -x!MIMER\*.svn -x!MIMERTests\bin -x!MIMERTests\obj -x!MIMERTests\*.*scc -x!MIMERTests\*.svn -x!_resharper* -x!*.resharper -r

7z a ..\MIMER_Binaries%1.zip ..\MIMER\bin\Release\*.*