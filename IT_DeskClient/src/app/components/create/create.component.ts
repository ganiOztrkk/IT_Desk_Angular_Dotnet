import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { FileUploadModule } from 'primeng/fileupload';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { ButtonModule } from 'primeng/button';

interface UploadEvent {
  originalEvent: Event;
  files: File[];
}

@Component({
  selector: 'app-create',
  standalone: true,
  imports: [
    InputTextModule,
    CommonModule,
    FormsModule,
    FileUploadModule,
    ToastModule,
    InputTextareaModule,
    ButtonModule
  ],
  templateUrl: './create.component.html',
  styleUrl: './create.component.css',
  providers: [MessageService]
})
export class CreateComponent {
  subject: string= "";

  uploadedFiles: any[] = [];

    constructor(private messageService: MessageService) {}

    onUpload(event:any) {
        for(let file of event.files) {
            this.uploadedFiles.push(file);
        }

        this.messageService.add({severity: 'info', summary: 'File Uploaded', detail: ''});
    }
}
