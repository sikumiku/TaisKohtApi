# TaisKohtApi

Soovime pakkuda toidukohtade põhimenüüde ja päevapakkumiste teenust. On olemas mitmeid kodulehti “päevapakkumised”, 
kus on kirjas toidukohtade päevapraed. Kuid tavaliselt sellistel lehtedel ei ole masinloetavaid andmeid ehk siis varianti, 
et saad saata API pihta mingi kuupäeva koos päevapraadidega andmete uuendamiseks ja saad sama API käest küsida tänaseid 
päevapraade. Andmeid võiks saada küsida restorani, kuupäeva, asukoha või otsisõna/otsisõnaosa põhiselt. Selline teenus 
lihtsustaks ka toidukohtade päevapakkumiste haldamist, kuna praegu peavad teenuse pakkujad vastavat informatsiooni 
uuendama mitmes erinevas kohas - nii erinevates portaalides, kui ka enda veebileheküljel. Samuti oleks toidukohtade kogu 
menüü haldamine ühes kohas.

Projektis osalejateks on:
- Sigrid Aasma
- Martin Kask
- Evelin Jõgi
- Marko Nõu

## Kasutusjuhend

- Kloonida kohalikku arvutisse repository aadressilt https://github.com/sikumiku/TaisKohtApi
- Installida NodeJS
- ```npm install webpack -g```
- ```npm install -cli -g```
- Frontend rakenduse kasutamiseks valida Startup meetoditest IIS Express, veebiteenuse ja Swaggeri kasutamiseks käivitada Api Swagger

## Projekti tehnilise poole kohta

- Frontend on üles ehitatud Reacti peal
- Backend kasutab Asp.net Core 2.0 raamistikku 
- Turvalisus on üles ehitatud Identity raamistiku peale
- Admin kasutaja loomiseks logida sisse e-mailiga admin@gmail.com (soovitatav parool peab sisaldama suurt tähte, väikest tähte, numbrit ja kirjamärki, a la Aa12345678.)
- Rollid luuakse programmi käivitamisel (normalUser, premiumUser, admin)
- Kui IIS Express annab errori puuduva mooduli kohta, tuleb teha projekti alamprojektis TaisKohtApi ```npm install```
