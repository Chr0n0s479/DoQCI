import subprocess
import os

GHOSTSCRIPT_PATH = r"C:\Program Files\gs\gs10.07.0\bin\gswin64.exe"


def compress_pdf(input_path: str, output_path: str):

    command = [
        GHOSTSCRIPT_PATH,
        "-sDEVICE=pdfwrite",
        "-dCompatibilityLevel=1.4",
        "-dPDFSETTINGS=/screen",
        "-dNOPAUSE",
        "-dQUIET",
        "-dBATCH",
        f"-sOutputFile={output_path}",
        input_path
    ]
    print()

    result = subprocess.run(command, capture_output=True)

    if result.returncode != 0:
        return False


    outputName = os.path.basename(output_path)

    new_output_path = os.path.join(os.path.dirname(output_path), outputName.replace('_','',1))

    # os.replace(temp_output, input_path)
    os.remove(input_path)
    os.rename(output_path, new_output_path)