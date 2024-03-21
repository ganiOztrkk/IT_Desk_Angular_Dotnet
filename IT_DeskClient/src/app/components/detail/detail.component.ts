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

@Component({
  selector: 'app-detail',
  standalone: true,
  imports: [CommonModule, CardModule, ButtonModule, FormsModule],
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
    private error: ErrorService
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

  closeTicket(){
    this.http.get("").subscribe({
      next: () => {
        this.getTicket();
      }
    });
  }
}