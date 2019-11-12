import React from "react";
import ReactDOM from 'react-dom';
import { App } from './App';
import { mergeStyles } from 'office-ui-fabric-react';

export function initApp( elementId: string = 'app', url: string = '') {
  // Inject some global styles
  mergeStyles({
    selectors: {
      ':global(body), :global(html), :global(#app)': {
        margin: 0,
        padding: 0,
        height: '100vh'
      }
    }
  });

  ReactDOM.render(<App saveUrl={url} />, document.getElementById(elementId));
}