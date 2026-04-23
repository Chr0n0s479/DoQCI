import { Component } from '@angular/core';
import { FileUploader } from '../../components/file-uploader/file-uploader';
import { UploadJobResponse } from '../../models/upload-job-response';
import { WorkArea } from '../../components/work-area/work-area';

@Component({
  selector: 'app-merge',
  imports: [FileUploader, WorkArea],
  templateUrl: './merge.html',
})
export class Merge {

  job?: UploadJobResponse
  isLoading = false

  onUploadStarted() {
    this.isLoading = true
  }

  onFilesSelected(job: UploadJobResponse) {
    console.log("EVENT RECEIVED", job)

    this.job = job
    this.isLoading = false
    console.log(this.isLoading)
  }
}
