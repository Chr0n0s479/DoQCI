import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { UploadJobResponse } from '../models/upload-job-response'
import { ProcessFile } from '../models/process-file'
import { environment } from '../../environment/environment'
import { FileResponse } from '../models/file-response'

@Injectable({
  providedIn: 'root',
})
export class FileService {


  constructor(private http: HttpClient) {}

  upload(files: File[]): Observable<UploadJobResponse> {

    const formData = new FormData()

    files.forEach(file => {
      formData.append('Files', file)
    })

    return this.http.post<UploadJobResponse>(
      `${environment.apiPdf}/upload`,
      formData
    )
  }

  process(fileInfo: ProcessFile): Observable<FileResponse>{
    return this.http.post<FileResponse>(
    `${environment.apiPdf}/process`,
    fileInfo
    )
  }

}