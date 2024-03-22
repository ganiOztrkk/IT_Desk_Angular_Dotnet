import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { AuthService } from '../../services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { TicketModel } from '../../models/ticket.model';
import { FormsModule } from '@angular/forms';
import { TicketDetailModel } from '../../models/ticket-detail.model';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ErrorService } from '../../services/error.service';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';

@Component({
  selector: 'app-detail',
  standalone: true,
  imports: [CommonModule, CardModule, ButtonModule, FormsModule, ToastModule],
  templateUrl: './detail.component.html',
  styleUrl: './detail.component.css'
})
export default class DetailComponent {
  details: TicketDetailModel[] = [];
  ticket: TicketModel = new TicketModel();
  ticketCreaterName: string = "";
  ticketCreaterLastName: string = "";

  content: string = "";
  ticketId: string = "";

  constructor(
    public auth: AuthService,
    private activated: ActivatedRoute,
    private http: HttpClient,
    private error: ErrorService,
    private messageService: MessageService
  ){
    this.activated.params.subscribe((res)=> {
      this.ticketId = res["value"];
      this.getTicket();
      this.getDetails();
    })
  }

  getTicket(){
     this.http.get("http://localhost:5180/api/Tickets/GetById/by-id/"+ this.ticketId, {
      headers: {
        "Authorization":"Bearer " + this.auth.accessToken
      }
    })
    .subscribe({
      next: (res: any) => {
        this.ticket = res;
        this.ticketCreaterName = res.appUser.name;
        this.ticketCreaterLastName = res.appUser.lastName;
        console.log(this.ticket);
      },
      error: (err: HttpErrorResponse) => {
        this.error.errorHandler(err);
      }
    })
  }

  getDetails(){
    this.http.get("http://localhost:5180/api/Tickets/GetDetails/"+ this.ticketId, {
      headers: {
        "Authorization":"Bearer " + this.auth.accessToken
      }
    })
    .subscribe({
      next: (res: any) => {
        this.details = res;
      },
      error: (err: HttpErrorResponse) => {
        this.error.errorHandler(err);
      }
    })
  }

  addDetailContent(){
    const data = {
      appUserId: this.auth.userId,
      content: this.content,
      ticketId: this.ticketId
    }
    if (this.content == "") {
      this.messageService.add({
        severity: 'error',
        summary: "Mesaj boş olamaz.",
        detail: 'Mesaj alanına talebinizi yazın',
      });
    }
    else{
      this.http.post("http://localhost:5180/api/Tickets/AddDetailContent", data,{
        headers: {
          "Authorization":"Bearer " + this.auth.accessToken
        }
      }).subscribe({
        next: () => {
        this.content = "";
        this.getDetails();
        this.getTicket();
        }
      })
    }
  }

  closeTicket(){
    this.http.get("http://localhost:5180/api/Tickets/Close/close-ticket/"+ this.ticketId, {
      headers: {
        "Authorization":"Bearer " + this.auth.accessToken
      }
    }).subscribe({
      next: () => {
        this.getTicket();
      }
    });
  }
}