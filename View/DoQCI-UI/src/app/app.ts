import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FileUploader } from "./components/file-uploader/file-uploader";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('DoQCI-UI');
}
