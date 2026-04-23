import { Component, Output, EventEmitter, Input } from '@angular/core';
import { FileService } from '../../services/file.service';

@Component({
  selector: 'app-file-uploader',
  imports: [],
  templateUrl: './file-uploader.html',
})
export class FileUploader {


  constructor(private fileService: FileService) {}

  @Input() multiple = false;
  @Input() accept = ".pdf";

  files: any[] = [];

  @Output() filesSelected = new EventEmitter<any[]>();

  onFileSelected(event: Event) {

    const input = event.target as HTMLInputElement;

    console.log(input)
    if (!input.files) return;

    const selectedFiles = Array.from(input.files);
    this.fileService.upload(selectedFiles)
      .subscribe(res => {

        this.files = res;

        this.filesSelected.emit(this.files);

      });

  }
}
