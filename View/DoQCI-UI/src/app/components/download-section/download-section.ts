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
  @Output() close = new EventEmitter<void>()       

  download() {
    window.open(
      `${environment.apiPdf}/download?path=${this.path.replace('/storage/', '')}`,
      "_blank"
    )
  }

  continueProcessing() {
    this.close.emit()   
  }

  processAnother() {
    this.reset.emit()  
  }
}