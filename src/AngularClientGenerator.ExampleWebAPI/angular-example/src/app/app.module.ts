import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { ExampleGeneratedModule } from './generated';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    ExampleGeneratedModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
