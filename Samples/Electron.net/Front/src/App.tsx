import React, { Component, ReactNode } from 'react';
import {initializeIcons} from '@uifabric/icons';
import { MeetingNotes } from './MeetingNotesComponent';

initializeIcons();


export class App extends Component {
  constructor(props: Readonly<{}>) {
    super(props);

  }

  render(): ReactNode {
    return (<MeetingNotes meetingNotes={{}}></MeetingNotes>);
  }
};
