import { TicketFileUrlModel } from "./ticket-file-url.model";

export class TicketModel{
    id: string = "";
    subject: string = "";
    createDate: string = "";
    isActive: boolean = true;
    appUserId: string = "";
    appUser: any;
    fileUrl: TicketFileUrlModel[] = [];
}