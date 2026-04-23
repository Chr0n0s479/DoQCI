import { Component } from '@angular/core';
import { FileUploader } from '../../components/file-uploader/file-uploader';

@Component({
  selector: 'app-merge',
  imports: [FileUploader],
  templateUrl: './merge.html',
})
export class Merge {
  files: File[] = [];
  onFilesSelected(files: File[]) {
    this.files = files;
  }
}
