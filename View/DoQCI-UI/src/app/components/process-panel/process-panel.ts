import { Component, EventEmitter, Output } from '@angular/core'
import { FormsModule } from '@angular/forms'
import { ProcessOptions } from '../../models/process-options'

@Component({
  selector: 'app-process-panel',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './process-panel.html'
})
export class ProcessPanel {

  compress = false
  enhance = false
  ocr = false

  @Output() process = new EventEmitter<ProcessOptions>()

  onProcess() {

    const options: ProcessOptions = {
      compress: this.compress,
      enhance: this.enhance,
      ocr: this.ocr
    }

    this.process.emit(options)

  }

}