import { Component, OnInit, ViewChild } from '@angular/core';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import {
  DialogService,
  DynamicDialogModule,
  DynamicDialogRef,
} from 'primeng/dynamicdialog';
import { MessageService } from 'primeng/api';
import { CreateComponent } from '../create/create.component';
import { TicketModel } from '../../models/ticket.model';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { DatePipe } from '@angular/common';
import { ToastModule } from 'primeng/toast';
import { ErrorService } from '../../services/error.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    BreadcrumbModule,
    TableModule,
    TagModule,
    InputTextModule,
    ButtonModule,
    DynamicDialogModule,
    ToastModule
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  providers: [DialogService, MessageService, DatePipe],
})
export default class HomeComponent implements OnInit{

  @ViewChild('dt1') dt1: any;
  tickets : TicketModel[] = [];
  selectedTickets: TicketModel = new TicketModel();
  ref: DynamicDialogRef | undefined;

  constructor(
    public dialogService: DialogService,
    public messageService: MessageService,
    private http: HttpClient,
    public auth: AuthService,
    public date: DatePipe,
    private error: ErrorService,
    private route: Router
  ) {}


  ngOnInit(): void {
    this.getAllTickets();
  }

  getAllTickets(){
    this.http.get("http://localhost:5180/api/Tickets/GetAll", {
      headers: {
        "Authorization":"Bearer " + this.auth.accessToken
      }
    })
    .subscribe({
      next: (res: any) => {
        this.tickets = res;
      },
      error: (err: HttpErrorResponse) => {
        this.error.errorHandler(err);
      }
    })
  }

  show() {
    this.ref = this.dialogService.open(CreateComponent, {
      header: 'Destek Oluştur',
      width: '50vw',
      contentStyle: { overflow: 'auto' },
      baseZIndex: 10000,
      maximizable: false,
      breakpoints: {
        '960px': '75vw',
        '640px': '90vw',
      },
    });

    this.ref.onClose.subscribe((data: any) => {
      if (data) {
        this.http.post("http://localhost:5180/api/Tickets/Add", data, {
          headers: {
            "Authorization":"Bearer " + this.auth.accessToken
          }
        }).subscribe({
          next: (res: any) => {
            this.getAllTickets();
            this.messageService.add({
              severity: 'success',
              summary: "İşlem başarılı",
              detail: res.message,
            });
          },
          error: (err :HttpErrorResponse) => {
            this.error.errorHandler(err);
          }
        })
      }
    });

    this.ref.onMaximize.subscribe((value) => {
      this.messageService.add({
        severity: 'info',
        summary: 'Maximized',
        detail: `maximized: ${value.maximized}`,
      });
    });
  }

  ngOnDestroy() {
    if (this.ref) {
      this.ref.close();
    }
  }

  getSeverity(status: boolean) {
    switch (status) {
      case false:
        return 'danger';

      case true:
        return 'success';

      /* case 'new':
        return 'info';

      case 'negotiation':
        return 'warning'; */

      default:
        return 'danger';
    }
  }

  applyFilter(event: Event, field: string) {
    const input = event.target as HTMLInputElement;
    const value = input.value; // Artık "value" kesinlikle string olarak kabul edilecektir.
    // Filter uygulama işlemi
    this.dt1.filter(value, field, 'contains');
}

  goTicketDetail(){
  try {
    this.route.navigateByUrl("ticket-details/" + this.selectedTickets.id)
  } catch (error) {
    return;
  }
  
}

}