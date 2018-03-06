#ifndef __MENU_SEBCE_H__
#define __MENU_SEBCE_H__

#include "cocos2d.h"

class MenuSence : public cocos2d::Layer
{
public:
    static cocos2d::Scene* createScene();

    virtual bool init();
    
    // a selector callback
    //void startMenuCallback(cocos2d::Ref* pSender);
    
    // implement the "static create()" method manually
    CREATE_FUNC(MenuSence);

	void gameStart(cocos2d::Ref* pSender);
};

#endif // __MENU_SEBCE_H__
