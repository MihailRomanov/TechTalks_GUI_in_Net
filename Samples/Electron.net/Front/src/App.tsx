import React, { Component, ReactNode } from 'react';
import {initializeIcons} from '@uifabric/icons';
import { MeetingNotes } from './MeetingNotesComponent';

initializeIcons();

export interface IAppProps {
  saveUrl: string
}

export interface IAppState {
  saveUrl: string
}

export class App extends Component<IAppProps, IAppState> {
  constructor(props: IAppProps) {
    super(props);
    const { saveUrl } = props;
    this.state = {saveUrl};
  }

  render(): ReactNode {
    return (<MeetingNotes saveUrl={this.state.saveUrl} meetingNotes={{}}></MeetingNotes>);
  }
};
