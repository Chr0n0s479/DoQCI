import { ChangeDetectorRef, Component } from '@angular/core';
import { FileUploader } from '../../components/file-uploader/file-uploader';
import { UploadJobResponse } from '../../models/upload-job-response';
import { WorkArea } from '../../components/work-area/work-area';
import { ProcessPanel } from '../../components/process-panel/process-panel';
import { ProcessOptions } from '../../models/process-options';
import { PageItem } from '../../models/page-item';
import { FileService } from '../../services/file.service';
import { LoadingSpinner } from "../../components/loading-spinner/loading-spinner";
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-home',
  imports: [FileUploader, WorkArea, ProcessPanel, LoadingSpinner],
  templateUrl: './home.html',
})
export class Home {

  job?: UploadJobResponse
  isLoading = false
  isProcessing = false
  pages: PageItem[] = []

  onPagesChanged(pages: PageItem[]) {

    this.pages = pages

  }

  constructor(private fileService: FileService, private cdr: ChangeDetectorRef) { 
    
  }
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
    if (!this.job?.jobId || pagesToProcess.length === 0)
      return;
    this.isProcessing = true;
    const payload = {

      pages: pagesToProcess,
      options: options,
      jobId: this.job.jobId

    }

    this.fileService.process(payload)
      .pipe(
        finalize(() =>{
          this.isProcessing = false 
          this.cdr.detectChanges();
        }
      )
          
      )
      .subscribe({

        next: (res) => {
          console.log(res)
        },

        error: err => console.error(err)

      });

    console.log(payload)

  }
}
