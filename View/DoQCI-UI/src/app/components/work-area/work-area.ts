import { Component, Input, Output, OnInit, EventEmitter } from '@angular/core';
import { DragDropModule, CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { UploadJobResponse } from '../../models/upload-job-response';
import { environment } from '../../../environment/environment';
import { NgClass } from '@angular/common';
import { PageItem } from '../../models/page-item';
@Component({
  selector: 'app-work-area',
  standalone: true,
  imports: [DragDropModule, NgClass],
  templateUrl: './work-area.html',
})
export class WorkArea implements OnInit {
  

  @Input() job!: UploadJobResponse;
  @Output() pagesChanged = new EventEmitter<PageItem[]>();  

  pages: PageItem[] = [];

  ngOnInit() {
    this.pages = this.job.files.flatMap(file =>
      file.pages.map(page => ({
        id: `${file.fileIndex}-${page.pageNumber}`,
        jobId: this.job.jobId,
        fileIndex: file.fileIndex,
        pageNumber: page.pageNumber,
        thumbnail: `${environment.storageUrl}/storage/temp/jobs/${this.job.jobId}/thumbs/file_${file.fileIndex}/${page.thumbnail}`,
        isEnabled: true
      }))
    )
    this.pagesChanged.emit(this.pages)
  }
  togglePage(page: any) {
    page.isEnabled = !page.isEnabled
    this.pagesChanged.emit(this.pages)
  }

  drop(event: CdkDragDrop<PageItem[]>) {
    moveItemInArray(this.pages, event.previousIndex, event.currentIndex);
    this.pagesChanged.emit(this.pages)
  }
  
  // work-area.ts
fileColors: Record<number, { border: string; badge: string }> = {};

private tailwindPairs = [
  { border: 'border-red-500',    badge: 'bg-red-500' },
  { border: 'border-blue-500',   badge: 'bg-blue-500' },
  { border: 'border-green-500',  badge: 'bg-green-500' },
  { border: 'border-purple-500', badge: 'bg-purple-500' },
  { border: 'border-orange-500', badge: 'bg-orange-500' },
  { border: 'border-pink-500',   badge: 'bg-pink-500' },
  { border: 'border-teal-500',   badge: 'bg-teal-500' },
  { border: 'border-yellow-500', badge: 'bg-yellow-500' },
  { border: 'border-indigo-500', badge: 'bg-indigo-500' },
  { border: 'border-cyan-500',   badge: 'bg-cyan-500' }
];

private getRandomPair() {
  const i = Math.floor(Math.random() * this.tailwindPairs.length);
  return this.tailwindPairs[i];
}

getFileColor(fileIndex: number) {
  if (!this.fileColors[fileIndex]) {
    this.fileColors[fileIndex] = this.getRandomPair();
  }
  return this.fileColors[fileIndex].border;
}

getBadgeColor(fileIndex: number) {
  if (!this.fileColors[fileIndex]) {
    this.fileColors[fileIndex] = this.getRandomPair();
  }
  return this.fileColors[fileIndex].badge;
}
}