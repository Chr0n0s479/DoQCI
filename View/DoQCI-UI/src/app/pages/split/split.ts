import { Component } from '@angular/core';
import { FileUploader } from '../../components/file-uploader/file-uploader';
@Component({
  selector: 'app-split',
  // imports: [FileUploader],
  templateUrl: './split.html',
})
export class Split {
  files: File[] = [];
onFilesSelected(files: File[]) {
  this.files = files;
}
}
