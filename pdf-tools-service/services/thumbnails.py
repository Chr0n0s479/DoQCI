import fitz
import os


def generate_thumbnails(pdf_path: str, output_folder: str, zoom: float = 0.2):

    os.makedirs(output_folder, exist_ok=True)

    doc = fitz.open(pdf_path)
    thumbnails = []

    matrix = fitz.Matrix(zoom, zoom)

    for page_number in range(len(doc)):

        page = doc.load_page(page_number)
        pix = page.get_pixmap(matrix=matrix)

        file_name = f"page_{page_number+1}.png"
        file_path = os.path.join(output_folder, file_name)

        pix.save(file_path)

        thumbnails.append({
            "PageNumber": page_number + 1,
            "Thumbnail": file_name
        })

    doc.close()

    return thumbnails