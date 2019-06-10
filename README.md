# Projects

# Project title

Aplikacja do zarządzania listą pracowników firmy "X".

## Source

XMLData.xml (umieszczony w bieżącym katalogu aplikacji)

## Instruction before run

Z uwagi na to, że w pliku XML odwołuję się do plików .png znajdujących się w katalogu "bin\Debug\Ikony" rozwiązania, konieczna jest 
korekta w pliku XML (tylko dla pierwszego załadowania), tj: zawartość tagów <ImageSource> należy uzupełnić ścieżką do katalogu 
bieżącego (z poziomu aplikacji), czyli: System.IO.Directory.GetCurrentDirectory() (pozostała część ścieżki znajduję się już w pliku).

## Features

Aplikacja służy do:
* wyszukiwania pracowników,
* dodawania pracowników,
* edycji danych pracowników (dane wymagane to: imię, nazwisko, data urodzenia oraz płeć).

## Contribute

* Source Code: https://github.com/malgowar/Projects

## Author

Małgorzata Warczak
