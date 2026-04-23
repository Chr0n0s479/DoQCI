import { Component, Input, OnInit } from '@angular/core'
import { UploadJobResponse } from '../../models/upload-job-response'
import { environment } from '../../../environment/environment'

interface PageItem {
  jobId: string
  fileIndex: number
  pageNumber: number
  thumbnail: string
}

@Component({
  selector: 'app-work-area',
  standalone: true,
  templateUrl: './work-area.html',
})
export class WorkArea implements OnInit {

  @Input() job!: UploadJobResponse

  pages: PageItem[] = []

  ngOnInit() {
    this.buildPages()
  }

  buildPages() {

    this.pages = this.job.files.flatMap(file =>
      file.pages.map(page => ({
        jobId: this.job.jobId,
        fileIndex: file.fileIndex,
        pageNumber: page.pageNumber,
        thumbnail: `${environment.storageUrl}/storage/temp/jobs/${this.job.jobId}/thumbs/file_${file.fileIndex}/${page.thumbnail}`

      }))
    )
    console.log(this.pages)
  }

}