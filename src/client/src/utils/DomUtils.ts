type ElementLike = Document | Element;

export const scrollTop = (selector: string | ElementLike, base: ElementLike = document) => {
  const element: any = typeof selector === 'string' ? base.querySelector(selector) : selector;

  if (element && typeof element.scrollTo === 'function') {
    element.scrollTo(0, 0);
  }
};