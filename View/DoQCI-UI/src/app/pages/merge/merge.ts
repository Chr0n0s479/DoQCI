import { Component } from '@angular/core';
import { FileUploader } from '../../components/file-uploader/file-uploader';
import { UploadJobResponse } from '../../models/upload-job-response';
import { WorkArea } from '../../components/work-area/work-area';
import { ProcessPanel } from '../../components/process-panel/process-panel';
import { ProcessOptions } from '../../models/process-options';
import { PageItem } from '../../models/page-item';
import { FileService } from '../../services/file.service';

@Component({
  selector: 'app-merge',
  imports: [FileUploader, WorkArea, ProcessPanel],
  templateUrl: './merge.html',
})
export class Merge {

  job?: UploadJobResponse
  isLoading = false
  pages: PageItem[] = []

  onPagesChanged(pages: PageItem[]) {

    this.pages = pages

  }

  constructor(private fileService: FileService) {}
  onUploadStarted() {
    this.isLoading = true
  }

  onFilesSelected(job: UploadJobResponse) {
    console.log("EVENT RECEIVED", job)

    this.job = job
    this.isLoading = false
    console.log(this.isLoading)
  }
  onProcess(options: ProcessOptions) {

    const pagesToProcess = this.pages
      .filter(p => p.isEnabled)
      .map(p => ({
        fileIndex: p.fileIndex,
        pageNumber: p.pageNumber
      }))
    if(this.job?.jobId == undefined)
      return
    const payload = {

      pages: pagesToProcess,
      options: options,
      jobId: this.job.jobId

    }
    this.fileService.process(payload)  .subscribe({
        
        next: (res) => {

         

          console.log(res)
        },
        error: err => console.error(err)
      })

    console.log(payload)

  }
}
