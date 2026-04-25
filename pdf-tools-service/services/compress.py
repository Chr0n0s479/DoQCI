import subprocess

GHOSTSCRIPT_PATH = r"C:\Program Files\gs\gs10.07.0\bin\gswin64.exe"


def compress_pdf(input_path: str, output_path: str):

    command = [
        GHOSTSCRIPT_PATH,
        "-sDEVICE=pdfwrite",
        "-dCompatibilityLevel=1.4",
        "-dPDFSETTINGS=/ebook",
        "-dNOPAUSE",
        "-dQUIET",
        "-dBATCH",
        f"-sOutputFile={output_path}",
        input_path
    ]

    result = subprocess.run(command, capture_output=True)

    return result.returncode == 0