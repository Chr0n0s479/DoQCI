export interface UploadJobResponse {
  jobId: string
  files: FileInfoResponse[]
}

export interface FileInfoResponse {
  fileIndex: number
  fileName: string
  pagesCount: number
  pages: PageInfoResponse[]
}

export interface PageInfoResponse {
  pageNumber: number
  thumbnail: string
}