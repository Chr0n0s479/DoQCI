import { Component } from '@angular/core';
import { FileUploader } from '../../components/file-uploader/file-uploader';

@Component({
  selector: 'app-merge',
  imports: [FileUploader],
  templateUrl: './merge.html',
  styleUrl: './merge.css',
})
export class Merge {
    onFilesSelected($event: Event) {
    throw new Error('Method not implemented.');
  }
}
