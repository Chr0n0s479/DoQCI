import { Component, Output, EventEmitter, Input } from '@angular/core'
import { FileService } from '../../services/file.service'
import { UploadJobResponse } from '../../models/upload-job-response'
import { EventContract } from '@angular/core/primitives/event-dispatch'

@Component({
  selector: 'app-file-uploader',
  standalone: true,
  templateUrl: './file-uploader.html',
})
export class FileUploader {

  constructor(private fileService: FileService) {}

  @Input() multiple = false
  @Input() accept = ".pdf"

  job?: UploadJobResponse

  @Output() filesSelected = new EventEmitter<UploadJobResponse>()
  @Output() uploadStarted = new EventEmitter<void>()
  @Output() uploadFinished = new EventEmitter<UploadJobResponse>()

  onFileSelected(event: Event) {

    const input = event.target as HTMLInputElement
    console.log('changed')
    if (!input.files) return
    const selectedFiles = Array.from(input.files)
    this.uploadStarted.emit()
    this.fileService.upload(selectedFiles)
      .subscribe({
        
        next: (res) => {

          this.job = res

          // this.filesSelected.emit(res)
          this.uploadFinished.emit(res)
          console.log(res)
        },
        error: err => console.error(err)
      })

  }
}