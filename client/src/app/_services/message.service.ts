import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';
import { Message } from '../_models/message';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { User } from '../_models/user';
import { BehaviorSubject, take } from 'rxjs';
import { Group } from '../_models/group';
import { BusyLoadingService } from './busy-loading.service';


@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl
  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;
  private messageThreadSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();

  constructor(private http: HttpClient, private busyLoadingService: BusyLoadingService) { }

  createHubConnection(user: User, otherUsername: string) {
    // this.busyLoadingService.busy();
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'message?user=' + otherUsername, {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start()
    .catch(error => console.log(error));
    // .finally(() => this.busyLoadingService.idle());

    this.hubConnection.on('ReceiveMessageThread', messages => {
      this.messageThreadSource.next(messages);
    });

    this.hubConnection.on('UpdatedGroup', (group: Group) => {
      if (group.connections.some(x => x.username === otherUsername)) {
        this.messageThread$.pipe(take(1)).subscribe({
          next: messages => {
            messages.forEach(message => {
              if (!message.dateRead) {
                message.dateRead = new Date(Date.now());
              }
            });
            this.messageThreadSource.next([...messages]);
          }
        });
      }
    });

    this.hubConnection.on('NewMessage', message => {
      this.messageThread$.pipe(take(1)).subscribe({
        next: messages => {
          // the spread operator (...) doesn't mutate the existing array, it creates a new array to replace the existing one
          this.messageThreadSource.next([...messages, message])
        }
      });
    });
  }

  stopHubConnection() {
    if (this.hubConnection) {
      this.messageThreadSource.next([]);
      this.hubConnection?.stop();
    }
  }

  getMessages(pageNumber: number, pageSize: number, container: string) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('Container', container);
    return getPaginatedResult<Message[]>(this.baseUrl + 'messages', params, this.http);
  }

  getMessageThread(username: string) {
    return this.http.get<Message[]>(this.baseUrl + 'messages/thread/' + username);
  }

  // invoke() will return a javascript Promise, but in case it doesn't, to force it to, the "async" keyword is used
  async sendMessage(username: string, content: string) {
    return this.hubConnection?.invoke('SendMessage', {recipientUsername: username, content})
      .catch(error => console.log(error));

    // recipientUsername must match what's in CreateMessageDto.cs from the API
    // return this.http.post<Message>(this.baseUrl + 'messages', { recipientUsername: username, content });
  }

  deleteMessage(id: number) {
    return this.http.delete(this.baseUrl + 'messages/' + id);
  }
}
