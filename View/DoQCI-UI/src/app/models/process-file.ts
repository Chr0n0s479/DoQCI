import { ProcessOptions } from "./process-options";

export interface ProcessFile{
    jobId: string,
    options: ProcessOptions,
    pages: {
        fileIndex: number,
        pageNumber: number
    }[]
}