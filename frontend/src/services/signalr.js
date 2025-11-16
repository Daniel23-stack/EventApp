import * as signalR from '@microsoft/signalr';

let connection = null;

export const startConnection = () => {
  const token = localStorage.getItem('token');
  
  connection = new signalR.HubConnectionBuilder()
    .withUrl('https://localhost:7000/inventoryhub', {
      accessTokenFactory: () => token || ''
    })
    .withAutomaticReconnect()
    .build();

  connection.start()
    .then(() => {
      console.log('SignalR Connected');
    })
    .catch((err) => {
      console.error('SignalR Connection Error:', err);
    });

  return connection;
};

export const stopConnection = () => {
  if (connection) {
    connection.stop();
    connection = null;
  }
};

export const getConnection = () => connection;

export const onInventoryUpdated = (callback) => {
  if (connection) {
    connection.on('InventoryUpdated', callback);
  }
};

export const onNewAlert = (callback) => {
  if (connection) {
    connection.on('NewAlert', callback);
  }
};

export const offInventoryUpdated = () => {
  if (connection) {
    connection.off('InventoryUpdated');
  }
};

export const offNewAlert = () => {
  if (connection) {
    connection.off('NewAlert');
  }
};

