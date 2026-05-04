import { Component, EventEmitter, Input, Output } from '@angular/core';
import { environment } from '../../../environment/environment';

@Component({
  selector: 'app-download-section',
  standalone: true,
  templateUrl: './download-section.html'
})
export class DownloadSection {

  @Input() fileName!: string
  @Input() path!: string

  @Output() reset = new EventEmitter<void>()

  download() {
  const link = document.createElement('a')

  link.href = environment.storageUrl + this.path
  link.download = 'DoQCI_processed_file.pdf'   

  document.body.appendChild(link)
  link.click()
  document.body.removeChild(link)
}

  processAnother() {
    this.reset.emit()
  }

}