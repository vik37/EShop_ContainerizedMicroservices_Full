export let errorMessagesByStatus = (statusCode: number, message: string = '')
  : string => {
    switch(statusCode){
      case 400:
        return 'Invalid Request';
      case 401:
          return 'You are not authorized for this page';
      case 404:
        return `${message}`;
      case 429:
        return 'Too many request. Please try later';
      default:
        return 'Problem at the server. Please try later';
  }
};
