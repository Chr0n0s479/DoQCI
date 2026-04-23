import fitz
import pytesseract
from PIL import Image
import io

pytesseract.pytesseract.tesseract_cmd = r"C:\Program Files\Tesseract-OCR\tesseract.exe"


def ocr_pdf(input_path: str, output_path: str, lang: str = "spa"):

    doc = fitz.open(input_path)

    for page in doc:

        pix = page.get_pixmap(dpi=300)

        img = Image.open(io.BytesIO(pix.tobytes()))

        text = pytesseract.image_to_string(img, lang=lang)

        page.insert_textbox(
            page.rect,
            text,
            fontsize=8,
            render_mode=3
        )

    doc.save(output_path)
    doc.close()