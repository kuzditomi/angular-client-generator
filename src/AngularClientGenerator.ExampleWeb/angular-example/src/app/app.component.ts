import { Component, OnInit } from '@angular/core';
import { GeneratedAngularClient } from './generated';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Angular example';
  responseMessage: Observable<string>;

  constructor(private client: GeneratedAngularClient.ExampleApiService){
  }
  
  ngOnInit(): void {
    this.responseMessage = this.client.ExampleMethod(1)
      .pipe(map(resp => resp.Message));
  }
}
