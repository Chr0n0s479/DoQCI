# DoQCI
A PDFToolKit for scholar purpose
venv\Scripts\activate
uvicorn main:app --reload --port 8000


You need to install python dependencies using requirements.txt
using "pip freeze > requirements.txt"

To could compress correctly pdf files, you need to install GhostScript: https://ghostscript.com/releases/

On angular root path you need to execute npm install

This Application allows to split, join, merge, compress pdf files

A early version of OCR has been implemented but does not work 100% as expected
