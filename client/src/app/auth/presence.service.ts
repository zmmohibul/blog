import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../interfaces/user';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;
  private onlineUsersSource = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this.onlineUsersSource.asObservable();

  constructor() { }

  createHubConnection(user: User) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.hubUrl}/presence`, {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .catch(err => {
        console.log(err);
      })

    this.hubConnection
      .on("UserIsOnline", (username) => {
        console.log(`${username} is online!`);
      });

    this.hubConnection
      .on("UserIsOffline", (username) => {
        console.log(`${username} is offline!`);
      });

    this.hubConnection.on('GetOnlineUsers', (usernames: string[]) => {
      this.onlineUsersSource.next(usernames);
    })
  }

  stopConnection() {
    if (this.hubConnection) {
      this.hubConnection
        .stop()
        .catch(err => {
          console.log(err);
        })
    }
  }
}
