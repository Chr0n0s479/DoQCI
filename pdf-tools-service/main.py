from fastapi import FastAPI, UploadFile, File
from fastapi.responses import FileResponse
import os
import shutil
import uuid
from pydantic import BaseModel


from services.compress import compress_pdf
from services.thumbnails import generate_thumbnails
from services.ocr import ocr_pdf

app = FastAPI()
STORAGE_ROOT = os.getenv("STORAGE_ROOT", "C:/DoQCI/storage")

TEMP_FOLDER = "temp"
os.makedirs(TEMP_FOLDER, exist_ok=True)

class ThumbnailRequest(BaseModel):
    jobId: str
    fileIndex: int


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


@app.post("/generate-thumbnails")
async def generate_thumbnails_endpoint(request: ThumbnailRequest):

    job_id = request.jobId
    file_index = request.fileIndex

    pdf_path = os.path.join(
        STORAGE_ROOT,
        "temp",
        "jobs",
        job_id,
        "files",
        f"file_{file_index}.pdf"
    )

    thumbs_folder = os.path.join(
        STORAGE_ROOT,
        "temp",
        "jobs",
        job_id,
        "thumbs",
        f"file_{file_index}"
    )

    thumbs = generate_thumbnails(pdf_path, thumbs_folder)

    return thumbs

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