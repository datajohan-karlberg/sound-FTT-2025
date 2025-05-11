# VbSpectrumAnalyzer

Ett VB.NET 4.8-program som visar spektrumanalys i realtid från mikrofonen.

## Funktioner

- Använder NAudio för att spela in ljud
- FFT-bearbetning med FftSharp
- Logaritmisk frekvensindelning (50 Hz till 12 kHz)
- Visar 8 tydliga staplar i ett Panel-fönster
- Kräver .NET Framework 4.8

## Skärmbild

![alt text](https://github.com/datajohan-karlberg/sound-FTT-2025/blob/master/Pict%202025-05-11%2007-57-48%20001.png)

## Kom igång

1. Klona repot:
   ```bash
   git clone https://github.com/datajohan-karlberg/VbSpectrumAnalyzer.git


## nuget prylar

Install-Package NWaves
Install-Package FftSharp
