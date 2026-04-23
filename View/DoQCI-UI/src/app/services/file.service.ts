import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { UploadJobResponse } from '../models/upload-job-response'

@Injectable({
  providedIn: 'root',
})
export class FileService {

  private apiUrl = 'https://localhost:7103/api/pdf'

  constructor(private http: HttpClient) {}

  upload(files: File[]): Observable<UploadJobResponse> {

    const formData = new FormData()

    files.forEach(file => {
      formData.append('Files', file)
    })

    return this.http.post<UploadJobResponse>(
      `${this.apiUrl}/upload`,
      formData
    )
  }

}