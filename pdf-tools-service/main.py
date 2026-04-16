from fastapi import FastAPI, UploadFile, File
from fastapi.responses import FileResponse
import os
import shutil
import uuid

from services.compress import compress_pdf
from services.thumbnails import generate_thumbnails
from services.ocr import ocr_pdf

app = FastAPI()

TEMP_FOLDER = "temp"
os.makedirs(TEMP_FOLDER, exist_ok=True)


@app.get("/health")
async def health():
    return {"status": "ok"}


@app.post("/compress")
async def compress(file: UploadFile = File(...)):
    input_name = f"{uuid.uuid4()}.pdf"
    output_name = f"{uuid.uuid4()}_compressed.pdf"

    input_path = os.path.join(TEMP_FOLDER, input_name)
    output_path = os.path.join(TEMP_FOLDER, output_name)

    with open(input_path, "wb") as buffer:
        shutil.copyfileobj(file.file, buffer)

    compress_pdf(input_path, output_path)

    return FileResponse(
        path=output_path,
        media_type="application/pdf",
        filename="compressed.pdf"
    )

@app.post("/thumbnails")
async def thumbnails(file: UploadFile = File(...)):

    input_name = f"{uuid.uuid4()}.pdf"
    pdf_path = os.path.join(TEMP_FOLDER, input_name)

    with open(pdf_path, "wb") as buffer:
        shutil.copyfileobj(file.file, buffer)

    thumbs_folder = os.path.join(TEMP_FOLDER, f"{input_name}_thumbs")

    thumbs = generate_thumbnails(pdf_path, thumbs_folder)

    return {
        "pages": thumbs
    }

@app.post("/ocr")
async def ocr(file: UploadFile = File(...)):

    input_name = f"{uuid.uuid4()}.pdf"
    output_name = f"{uuid.uuid4()}_compressed.pdf"

    input_path = os.path.join(TEMP_FOLDER, input_name)
    output_path = os.path.join(TEMP_FOLDER, output_name)

    with open(input_path, "wb") as buffer:
        shutil.copyfileobj(file.file, buffer)

    ocr_pdf(input_path, output_path, "spa")

    return FileResponse(
        path=output_path,
        media_type="application/pdf",
        filename="compressed.pdf"
    )